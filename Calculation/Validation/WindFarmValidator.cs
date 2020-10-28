using DataModel.Dto;
using FluentValidation;

namespace Calculation.Validation
{
    public class WindFarmValidator : AbstractValidator<WindFarm>
    {
        public WindFarmValidator()
        {
            RuleFor(wf => wf.Location).SetValidator(new LocationValidator());
            RuleFor(wf => wf.Turbines).NotEmpty();
            RuleForEach(wf => wf.Turbines).SetValidator(new TurbineValidator());
        }
    }
}