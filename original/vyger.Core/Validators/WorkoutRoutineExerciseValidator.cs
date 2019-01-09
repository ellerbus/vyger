using FluentValidation;
using vyger.Core.Models;

namespace vyger.Core.Validators
{
    public class WorkoutRoutineExerciseValidator : AbstractValidator<WorkoutRoutineExercise>
    {

        public WorkoutRoutineExerciseValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.WeekId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinWeeks, Constants.MaxWeeks);

            RuleFor(x => x.DayId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinDays, Constants.MaxDays);

            RuleFor(x => x.ExerciseId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(10)
                .Matches(Constants.Regex.IdPatterns.Exercise);

            RuleFor(x => x.WorkoutRoutine)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Matches(Constants.Regex.WorkoutPatterns.WorkoutSets);
        }
    }
}
