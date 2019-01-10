using FluentValidation;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Common.Validators
{
    public class CycleValidator : AbstractValidator<Cycle>
    {
        private ICycleService _service;

        public CycleValidator(ICycleService service)
        {
            _service = service;

            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(x => x.Weeks)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinWeeks, Constants.MaxWeeks);

            RuleFor(x => x.Days)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .InclusiveBetween(Constants.MinDays, Constants.MaxDays);
        }
    }
}