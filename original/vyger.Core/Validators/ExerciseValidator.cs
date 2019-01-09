using System.Linq;
using Augment;
using FluentValidation;
using vyger.Core.Models;
using vyger.Core.Services;

namespace vyger.Core.Validators
{
    public class ExerciseValidator : AbstractValidator<Exercise>
    {
        private IExerciseService _service;

        public ExerciseValidator(
            IExerciseService service)

        {
            _service = service;

            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Group)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEqual(ExerciseGroups.None);

            RuleFor(x => x.Category)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEqual(ExerciseCategories.None);

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(10)
                .Matches(Constants.Regex.IdPatterns.Exercise)
                .Must(ContainGroup).WithMessage("{PropertyName} must contain the Group")
                .Must(ContainCategory).WithMessage("{PropertyName} must contain the Category");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(1, 30)
                .Must(NotContainCategory).WithMessage("{PropertyName} not contain the category")
                .Must(BeUnique).WithMessage("{PropertyName} must be unique, {PropertyValue} already in use");
        }

        private bool ContainGroup(Exercise context, string arg)
        {
            string id = context.Group.GetIdentifier();

            return id == arg.Substring(0, 3);
        }

        private bool ContainCategory(Exercise context, string arg)
        {
            string id = context.Category.GetIdentifier();

            return id == arg.Substring(4, 2);
        }

        private bool NotContainCategory(Exercise context, string arg)
        {
            return arg.StartsWithNotSameAs(context.Category.ToString());
        }

        private bool BeUnique(Exercise context, string arg)
        {
            ExerciseCollection exercises = _service.GetExercises();

            bool exists = exercises.Any(x => x.Group == context.Group && x.Category == context.Category && x.Id != context.Id && x.Name.IsSameAs(arg));

            return !exists;
        }
    }
}
