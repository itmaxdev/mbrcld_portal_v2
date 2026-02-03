using Simple.OData.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Extensions
{
    internal static class SimpleODataClientExtensions
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> SelectPropertyNames
            = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> ExpandPropertyNames
            = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        internal static IBoundClient<T> ProjectToModel<T>(this IBoundClient<T> boundClient) where T : class
        {
            var propertiesForSelect = GetPropertiesForSelect(typeof(T));
            var propertiesForExpand = GetPropertiesForExpand(typeof(T));

            var selectExpressions = propertiesForSelect
                .Select(x => ODataDynamic.ExpressionFromReference(x.GetODataMemberName()))
                .ToArray();

            var expandExpressions = propertiesForExpand.Select(
                x => ODataDynamic.ExpressionFromReference(x.GetODataMemberName())).ToArray();

            var navigationPropertiesSelectExpressions = propertiesForExpand
                .Where(x => !IsCollectionNavigationPropertyType(x.PropertyType))
                .Select(x => ODataDynamic.ExpressionFromReference(x.GetODataMemberName()))
                .ToArray();

            var collectionNavigationPropertiesSelectExpressions = propertiesForExpand
                .Where(x => IsCollectionNavigationPropertyType(x.PropertyType))
                .SelectMany(
                    x => GetPropertiesForSelect(x.PropertyType).Select(
                        y => ODataDynamic.ExpressionFromReference($"{x.GetODataMemberName()}/{y.GetODataMemberName()}"))).ToArray();

            return boundClient
                .Expand(expandExpressions)
                .Select(selectExpressions)
                .Select(navigationPropertiesSelectExpressions)
                .Select(collectionNavigationPropertiesSelectExpressions);
        }

        private static string GetODataMemberName(this MemberInfo propertyInfo)
            => propertyInfo.GetCustomAttribute<DataMemberAttribute>()?.Name ?? propertyInfo.Name;

        private static IEnumerable<PropertyInfo> GetPropertiesForSelect(Type type)
        {
            return SelectPropertyNames.GetOrAdd(type, (requestedType) =>
            {
                if (IsCollectionNavigationPropertyType(requestedType))
                {
                    requestedType = requestedType.GetGenericArguments().First();
                }

                var executingAssembly = Assembly.GetExecutingAssembly();

                var properties = requestedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => x.CanRead)
                    .Where(x => x.CanWrite)
                    .Where(x => x.GetCustomAttribute<DataMemberAttribute>() != null)
                    .Where(x => x.PropertyType.Assembly.FullName != executingAssembly.FullName)
                    .ToList();

                return properties;
            });
        }

        private static IEnumerable<PropertyInfo> GetPropertiesForExpand(Type type)
        {
            return ExpandPropertyNames.GetOrAdd(type, (_) =>
            {
                var executingAssembly = Assembly.GetExecutingAssembly();

                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => x.CanRead)
                    .Where(x => x.CanWrite)
                    .Where(x => x.GetCustomAttribute<DataMemberAttribute>() != null)
                    .Where(x => PropertyTypeSupportsExpand(x.PropertyType, executingAssembly))
                    .ToList();

                return properties;
            });
        }

        private static bool PropertyTypeSupportsExpand(Type type, Assembly executingAssembly)
        {
            return (type.IsClass && type.Assembly.FullName == executingAssembly.FullName) ||
                IsCollectionNavigationPropertyType(type);
        }

        private static bool IsCollectionNavigationPropertyType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
        }
    }
}
