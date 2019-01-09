using System.Linq;
using Augment;
using FluentValidation;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Common.Validators
{
    ///	<summary>
    ///	Represents a basic validator for Exercise
    ///	</summary>
    public class ExerciseValidator : AbstractValidator<Exercise>
    {
        private IExerciseService _exercises;

        public ExerciseValidator(IExerciseService exercises)
        {
            _exercises = exercises;

            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Group)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEqual(ExerciseGroups.None);

            RuleFor(x => x.Category)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEqual(ExerciseCategories.None);

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .Length(1, 150)
                .Must(BeUnique).WithMessage("Name must be unique");
        }

        private bool BeUnique(Exercise context, string arg)
        {
            bool duplicate = _exercises.GetExerciseCollection()
                .Filter(context.Group, context.Category)
                .Where(x => x.Name.IsSameAs(arg))
                .Any(x => x.Id != context.Id);

            return !duplicate;
        }
    }
}