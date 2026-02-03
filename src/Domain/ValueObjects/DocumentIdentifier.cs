using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;

namespace Mbrcld.Domain.ValueObjects
{
    public sealed class DocumentIdentifier : ValueObject
    {
        public string Value { get; }

        private DocumentIdentifier(string value)
        {
            this.Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public static Result<DocumentIdentifier> FromValue(string value)
        {
            return new DocumentIdentifier(value);
        }

        public static readonly DocumentIdentifier EducationCertificate = new DocumentIdentifier("education-certificate");
        public static readonly DocumentIdentifier CurriculumVitae = new DocumentIdentifier("curriculum-vitae");
        public static readonly DocumentIdentifier PassportFrontPage = new DocumentIdentifier("passport-front-page");
        public static readonly DocumentIdentifier PassportBackPage = new DocumentIdentifier("passport-back-page");
        public static readonly DocumentIdentifier IdentityFrontPage = new DocumentIdentifier("identity-front-page");
        public static readonly DocumentIdentifier IdentityBackPage = new DocumentIdentifier("identity-back-page");
        public static readonly DocumentIdentifier FamilyFrontPage = new DocumentIdentifier("family-front-page");
        public static readonly DocumentIdentifier FamilyBackPage = new DocumentIdentifier("family-back-page");
    }
}
