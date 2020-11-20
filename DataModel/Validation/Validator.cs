using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Dto = DataModel.Dto;
using Validated = DataModel.Dto.Validated;

namespace Calculation.Validation
{
    public class Validator
    {
        private readonly Mapper mapper;

        public Validator()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<DataModel.ModelMappingProfile>();
            });

            this.mapper = new Mapper(config);
        }

        public Validated.WindFarm ValidateOrThrow(Dto.WindFarm windFarmDto)
        {
            var validator = new WindFarmValidator();
            validator.ValidateAndThrow(windFarmDto);

            return this.mapper.Map<Validated.WindFarm>(windFarmDto);
        }

        public ValidationResult<Validated.WindFarm> Validate(Dto.WindFarm windFarmDto)
        {
            var validator = new WindFarmValidator();
            var result = validator.Validate(windFarmDto);

            if (result.IsValid)
            {
                return new ValidationSuccess<Validated.WindFarm>(this.mapper.Map<Validated.WindFarm>(windFarmDto));
            }

            return new ValidationError<Validated.WindFarm>(result.Errors);
        }

        public ValidationResult<Validated.Location> Validate(Dto.Location location)
        {
            var validator = new LocationValidator();
            var result = validator.Validate(location);
            if (!result.IsValid)
            {
                return new ValidationError<Validated.Location>(result.Errors);
            }

            return new ValidationSuccess<Validated.Location>(
                new Validated.Location(location.Latitude, location.Longitude));
        }

        public abstract class ValidationResult<T>
        {}

        public class ValidationSuccess<T> : ValidationResult<T>
        {
            public T Result { get; }

            public ValidationSuccess(T result)
            {
                Result = result;
            }
        }

        public class ValidationError<T> : ValidationResult<T>
        {
            public IEnumerable<ValidationFailure> Errors { get; }

            public ValidationError(IEnumerable<ValidationFailure> errors)
            {
                Errors = errors;
            }
        }

    }
}