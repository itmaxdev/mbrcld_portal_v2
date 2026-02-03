using Dawn;
using System;
using System.Collections.Generic;

namespace Mbrcld.SharedKernel.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public string Value { get; }

        public Email(string value)
        {
            Guard.Argument(value, nameof(value)).NotEmpty().NotWhiteSpace().Contains("@");

            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }

        public static implicit operator Email(string value)
        {
            return new Email(value);
        }

        public static Maybe<Email> Create(string email)
        {
            try
            {
                return new Email(email);
            }
            catch
            {
                return null;
            }
        }
    }
}
