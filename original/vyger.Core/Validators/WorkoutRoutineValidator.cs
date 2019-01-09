using System.Linq;
using Augment;
using FluentValidation;
using vyger.Core.Models;
using vyger.Core.Services;

namespace vyger.Core.Validators
{
    public class WorkoutRoutineValidator : AbstractValidator<WorkoutRoutine>
    {
        private IWorkoutRoutineService _service;

        public WorkoutRoutineValidator(IWorkoutRoutineService service)
        {
            _service = service;

            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(2);

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(1, 50)
                .Must(BeUnique).WithMessage("{PropertyName} must be unique, {PropertyValue} already in use");

            RuleFor(x => x.Weeks)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinWeeks, Constants.MaxWeeks);

            RuleFor(x => x.Days)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinDays, Constants.MaxDays);
        }

        private bool BeUnique(WorkoutRoutine context, string arg)
        {
            WorkoutRoutineCollection routines = _service.GetWorkoutRoutines();

            bool exists = routines.Any(x => x.Id != context.Id && x.Name.IsSameAs(arg));

            return !exists;
        }
    }
}
