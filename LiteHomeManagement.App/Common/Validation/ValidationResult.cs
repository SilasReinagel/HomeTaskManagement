using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Common
{
    public sealed class ValidationResult
    {
        public static ValidationResult Valid => new ValidationResult(new List<string>());

        public bool IsValid { get; }
        public IEnumerable<string> ValidationIssues { get; }

        public ValidationResult(string validationIssue)
            : this(new List<string> { validationIssue }) { }

        public ValidationResult(IEnumerable<string> validationIssues)
        {
            ValidationIssues = validationIssues.ToList();
            IsValid = !ValidationIssues.Any();
        }

        public override string ToString()
        {
            return IsValid ? "Valid" : $"Invalid: {IssuesMessage}";
        }

        public static implicit operator bool(ValidationResult result)
        {
            return result.IsValid;
        }

        public string IssuesMessage => string.Join(Environment.NewLine, ValidationIssues);
    }
}
