using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Mbrcld.Application.Interfaces;
using Mbrcld.Domain.Entities;
using Mbrcld.Web.Configuration;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Helpers;
using Mbrcld.Web.Identity;
using Mbrcld.Web.Services;
using Mbrcld.Web.Swagger;
using Mbrcld.Web.UAE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mbrcld.Web.Extensions
{
    internal static class ServiceExtensions
    {
        internal static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IPreferredLanguageService, PreferredLanguageService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IUAEService, UAEService>();
        }

        internal static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning((options) =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.ReportApiVersions = true;
            });
        }

        internal static void ConfigureAspNetCoreIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>((options) =>
            {
                options.Stores.MaxLengthForKeys = 128;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Password = new PasswordOptions()
                {
                    RequiredLength = 8,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = false,
                };
            })
                .AddRoles<Role>()
                .AddDefaultTokenProviders();
        }

        internal static void ConfigureIdentityServer(this IServiceCollection services)
        {
            services.AddIdentityServer((options) =>
            {
                options.Endpoints.EnableDiscoveryEndpoint = true;
                options.Endpoints.EnableTokenEndpoint = true;
                options.Endpoints.EnableTokenRevocationEndpoint = true;
                options.Endpoints.EnableUserInfoEndpoint = true;
                options.Endpoints.EnableAuthorizeEndpoint = false;
                options.Endpoints.EnableDeviceAuthorizationEndpoint = false;
                options.Endpoints.EnableCheckSessionEndpoint = false;
                options.Endpoints.EnableEndSessionEndpoint = false;
                options.Endpoints.EnableIntrospectionEndpoint = false;
                options.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(10);
                options.Caching.ResourceStoreExpiration = TimeSpan.FromMinutes(10);
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryCaching()
                .AddClientStoreCache<ClientStore>()
                .AddResourceStoreCache<ResourceStore>()
                .AddAspNetIdentity<User>()
                .Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        }

        internal static void ConfigureJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new IdentityServerSettings();
            configuration.GetSection("IdentityServerSettings").Bind(settings);

            services.AddAuthentication((options) =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer((options) =>
                {
                    options.Authority = settings.Authority;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidTypes = new[] { "at+jwt" },
                    };
                });
        }

        internal static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization((options) =>
            {
                options.AddPolicy(Policies.RequireAdmin, (builder) =>
                {
                    builder.RequireRole(Roles.Administrator);
                });
            });
        }

        internal static void ConfigureControllersWithViews(this IServiceCollection services)
        {
            services.AddControllersWithViews((options) =>
            {
                // Workaround for OData and Swagger
                // see https://github.com/OData/WebApi/issues/1177#issuecomment-358659774
                new ODataSwaggerMvcFix(options).Apply();
            })
                .AddFluentValidation()
                .AddNewtonsoftJson((options) =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .ConfigureApiBehaviorOptions((options) =>
                {
                    options.InvalidModelStateResponseFactory = (context) =>
                    {
                        return new InvalidModelStateResponseBuilder(context).Build();
                    };
                });

            services.Configure<MvcOptions>((options) =>
            {
                //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiProblemDetails), 400));
                //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiProblemDetails), 422));
                //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiProblemDetails), 500));
            });
        }

        internal static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerSettings = new IdentityServerSettings();
            configuration.GetSection("IdentityServerSettings").Bind(identityServerSettings);

            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo()
                {
                    Title = "MBRCLD API",
                    Version = "1.0",
                    Description = "A Web API for MBRCLD",
                    Contact = new OpenApiContact()
                    {
                        Name = "Dynamic Objects SARL",
                        Email = "info@dynamicobjects.net",
                        Url = new Uri("https://dynamicobjects.net"),
                    }
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            TokenUrl = new Uri($"{identityServerSettings.Authority}/connect/token"),
                            RefreshUrl = new Uri($"{identityServerSettings.Authority}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                [IdentityServerConstants.StandardScopes.OpenId] = "openid",
                                [IdentityServerConstants.StandardScopes.Profile] = "profile",
                                [IdentityServerConstants.StandardScopes.OfflineAccess] = "offline_access (Request a refresh token)",
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.OperationFilter<SwaggerParameterFilter>();

                options.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }

                    version = version.Replace("v", "");

                    var versionsSupportedByController = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    if (versionsSupportedByController.Any() &&
                        versionsSupportedByController.All(v => v.ToString() != version))
                    {
                        return false;
                    }

                    var versionsSupportedByAction = methodInfo.GetCustomAttributes(true)
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();

                    return versionsSupportedByAction.Any(v => v.ToString() == version) ||
                        (versionsSupportedByController.Count() < 2 && !versionsSupportedByAction.Any());
                });
            });
        }

        internal static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        internal static void ConfigureUrlHelper(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>((c) =>
            {
                var accessor = c.GetRequiredService<IActionContextAccessor>();
                var factory = c.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(accessor.ActionContext);
            });

            services.AddScoped<IUrlHelperService, UrlHelperService>();
        }

        internal static void AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UAEOptions>(configuration.GetSection("UAEOptions"));
        }
    }
}
