using DataModel.Dto;
using FluentValidation;

namespace Calculation.Validation
{
    public class TurbineValidator : AbstractValidator<Turbine>
    {
        public TurbineValidator()
        {
            RuleFor(t => t.BladeLength)
                .GreaterThan(0.0)
                .LessThan(t => t.TowerHeight).WithMessage("Blade length must be shorter than tower height");
                
            RuleFor(t => t.TowerHeight).GreaterThan(0.0);
            RuleFor(t => t.Location).SetValidator(new LocationValidator());
        }
    }
}