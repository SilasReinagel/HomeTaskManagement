using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteHomeManagement.App.Common
{
    public sealed class ReflectionValidation : IValidate<object>
    {
        private readonly AllowZeroes _zeroes;
        private readonly AllowEmptyCollections _collections;

        public ReflectionValidation(AllowZeroes zeroes = AllowZeroes.True, AllowEmptyCollections collections = AllowEmptyCollections.True)
        {
            _zeroes = zeroes;
            _collections = collections;
        }

        public ValidationResult Validate(object obj)
        {
            var issues = new List<string>();
            obj.GetProperties().Values.ToList().ForEach(prop => AddValidationIssues(obj, prop, issues));
            return new ValidationResult(issues);
        }

        private void AddValidationIssues(object obj, PropertyInfo prop, List<string> issues)
        {
            if (IsNull(obj, prop))
                issues.Add($"Missing required value '{prop.Name}'");
            else if (IsUnallowedZero(obj, prop))
                issues.Add($"Missing required value '{prop.Name}'");
            else if (IsEmptyString(obj, prop))
                issues.Add($"Missing required value '{prop.Name}'");
            else if (IsInvalidEmptyCollection(obj, prop))
                issues.Add($"Collection with required items is empty '{prop.Name}'");
            else if (IsValidatable(prop))
                AddItemValidationIssues(obj, prop, issues);
        }

        private void AddItemValidationIssues(object obj, PropertyInfo prop, List<string> issues)
        {
            obj.GetPropertyValue<IValidate>(prop.Name)
                .IfInvalid(x => issues.Add($"Invalid item '{prop.Name}': {x.IssuesMessage}"));
        }

        private bool IsValidatable(PropertyInfo prop)
        {
            return prop.PropertyType == typeof(IValidate);
        }

        private bool IsUnallowedZero(object obj, PropertyInfo prop)
        {
            return _zeroes == AllowZeroes.False 
                && prop.PropertyType.IsNumeric()
                && IsUnallowedZero(obj.GetPropertyValue(prop.Name));
        }

        private bool IsInvalidEmptyCollection(object obj, PropertyInfo prop)
        {
            return prop.PropertyType.IsEnumerable()
                   && _collections == AllowEmptyCollections.False
                   && !obj.GetPropertyValue<IEnumerable>(prop.Name).GetEnumerator().MoveNext();
        }

        private bool IsNull(object obj, PropertyInfo prop)
        {
            return obj.GetPropertyValue(prop.Name) == null;
        }

        private bool IsEmptyString(object obj, PropertyInfo prop)
        {
            return prop.PropertyType.IsString() && string.IsNullOrWhiteSpace(obj.GetPropertyValue<string>(prop.Name));
        }

        private bool IsUnallowedZero(object value)
        {
            return value is IConvertible && Convert.ToDecimal(value) < 0.0001m;
        }
    }
}
