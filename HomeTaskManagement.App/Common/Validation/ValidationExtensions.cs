using System;

namespace HomeTaskManagement.App.Common
{
    public static class ValidationExtensions
    {
        public static void IfInvalid(this IValidate obj, Action<ValidationResult> ifInvalid)
        {
            IfInvalid(obj.Validate(), ifInvalid);
        }

        public static void IfInvalid(this ValidationResult result, Action<ValidationResult> ifInvalid)
        {
            if (!result.IsValid)
                ifInvalid(result);
        }
    }
}
