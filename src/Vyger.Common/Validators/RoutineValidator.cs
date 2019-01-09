using System.Linq;
using Augment;
using FluentValidation;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Common.Validators
{
    public class RoutineValidator : AbstractValidator<Routine>
    {
        private IRoutineService _service;

        public RoutineValidator(IRoutineService service)
        {
            _service = service;

            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(1, 50)
                .Must(BeUnique).WithMessage("Name must be unique");

            RuleFor(x => x.Weeks)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinWeeks, Constants.MaxWeeks);

            RuleFor(x => x.Days)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinDays, Constants.MaxDays);

            RuleFor(x => x.Sets)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeFormatted);
        }

        private bool BeFormatted(Routine context, string[] arg)
        {
            if (arg != null)
            {
                //  != null for those UI's where Routine is part of
                //  the intrinsic validation with a bound property
                context.Sets = arg
                    .Where(x => x.IsNotEmpty())
                    .Select(x => WorkoutSet.Format(x))
                    .ToArray();
            }

            return true;
        }

        private bool BeUnique(Routine context, string arg)
        {
            bool duplicate = _service.GetRoutineCollection()
                .Where(x => x.Name.IsSameAs(arg))
                .Any(x => x.Id != context.Id);

            return !duplicate;
        }
    }
}