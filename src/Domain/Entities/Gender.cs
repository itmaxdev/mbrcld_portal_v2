using Dawn;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;

namespace Mbrcld.Domain.Entities
{
    public sealed class Gender : ValueObject
    {
        public static readonly Gender Male = new Gender(1);
        public static readonly Gender Female = new Gender(2);

        public int Value { get; }

        private Gender(int value)
        {
            Guard.Argument(value, nameof(value)).InRange(1, 2);

            this.Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public static Result<Gender> FromValue(int value)
        {
            try
            {
                return new Gender(value);
            }
            catch (Exception e)
            {
                return Result.Failure<Gender>(e.Message);
            }
        }

        public static implicit operator int(Gender gender)
        {
            return gender.Value;
        }

        public static implicit operator Gender(int value)
        {
            return new Gender(value);
        }
    }
}
