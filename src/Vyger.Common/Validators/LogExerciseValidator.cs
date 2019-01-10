using FluentValidation;
using Vyger.Common.Models;

namespace Vyger.Common.Validators
{
    ///	<summary>
    ///	Represents a basic validator for LogExercise
    ///	</summary>
    public class LogExerciseValidator : AbstractValidator<LogExercise>
    {
        public LogExerciseValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithMessage("Exercise is required");

            RuleFor(x => x.WorkoutPattern)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithMessage("Workout is required 'weight/reps, weight/reps' etc.");
        }
    }
}