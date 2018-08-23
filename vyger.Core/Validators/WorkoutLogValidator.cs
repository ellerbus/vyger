﻿using FluentValidation;
using vyger.Core.Models;

namespace vyger.Core.Validators
{
    public class WorkoutLogValidator : AbstractValidator<WorkoutLog>
    {

        public WorkoutLogValidator()
        {
            RuleFor(x => x.ExerciseId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(8)
                .Matches(Constants.Regex.IdPatterns.Exercise);

            RuleFor(x => x.Workout)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Matches(Constants.Regex.WorkoutPatterns.WorkoutSets);
        }
    }
}
