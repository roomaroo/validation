using FluentValidation;
using Dto = DataModel.Dto;

namespace Calculation.Validation
{
    internal class LocationValidator : AbstractValidator<Dto.Location>
    {
        public LocationValidator()
        {
            RuleFor(l => l.Latitude).InclusiveBetween(-90, 90);
            RuleFor(l => l.Longitude).InclusiveBetween(-180, 180);
        }
    }
}