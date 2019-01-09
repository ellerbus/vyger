using FluentValidation;
using vyger.Core.Models;

namespace vyger.Core.Validators
{
    public class WorkoutPlanCycleValidator : AbstractValidator<WorkoutPlanCycle>
    {

        public WorkoutPlanCycleValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.CycleId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .InclusiveBetween(Constants.MinCycles, Constants.MaxCycles);

            RuleFor(x => x.ExerciseId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(8)
                .Matches(Constants.Regex.IdPatterns.Exercise);

            RuleFor(x => x.Weight)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .InclusiveBetween(Constants.MinWeight, Constants.MaxWeight);

            RuleFor(x => x.Reps)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .InclusiveBetween(Constants.MinReps, Constants.MaxReps);

            RuleFor(x => x.Pullback)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .InclusiveBetween(Constants.MinPullback, Constants.MaxPullback);
        }
    }
}
