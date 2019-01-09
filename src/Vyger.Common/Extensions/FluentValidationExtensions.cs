using FluentValidation;
using FluentValidation.Results;

namespace Vyger.Common
{
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Performs validation and then throws an exception if validation fails.
        /// </summary>
        /// <param name="validator">The validator this method is extending.</param>
        /// <param name="instance">The instance of the type we are validating.</param>
        /// <param name="ruleSet">Optional: a ruleset when need to validate against.</param>
        public static void ValidateAndThrow<T>(this IValidator<T> validator, T instance)
        {
            ValidationResult result = validator.Validate(instance);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}