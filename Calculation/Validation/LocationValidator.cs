using DataModel.Dto;
using FluentValidation;

namespace Calculation.Validation
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(l => l.Latitude).InclusiveBetween(-90, 90);
            RuleFor(l => l.Longitude).InclusiveBetween(-180, 180);
        }
    }
}