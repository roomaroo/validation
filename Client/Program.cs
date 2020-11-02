using System;
using Calculation;
using Calculation.Validation;
using DataModel.Dto;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            WindFarm windFarm = new WindFarm
            {
                Location = new Location { Latitude = 51.453, Longitude = 2.6 },
                Turbines = new[] {
                    new Turbine
                    {
                        Id = Guid.NewGuid(),
                        TowerHeight = 80,
                        BladeLength = 60,
                        Location = new Location{ Latitude = 51.454, Longitude = 2.6}
                    },
                    new Turbine
                    {
                        TowerHeight = 80,
                        BladeLength = 100, // Blade will hit the ground
                        Location = new Location{ Latitude = 51.452, Longitude = 2.6}
                    }
                }
            };

            var calculation = new HeightCalculation();
            var validator = new Validator();

            try
            {
                var validated = validator.ValidateOrThrow(windFarm);
                Console.WriteLine($"Average turbine height: {calculation.AverageTurbineHeight(validated)}m");
            }
            catch (FluentValidation.ValidationException e)
            {
                PrintValidationException(e);
            }
            
            try
            {
                WindFarm emptyFarm = new WindFarm(); 
                var validated = validator.ValidateOrThrow(emptyFarm);
                Console.WriteLine($"Average turbine height in empty windfarm: {calculation.AverageTurbineHeight(validated)}m");
            }
            catch (FluentValidation.ValidationException e)
            {
                PrintValidationException(e);
            }

            try
            {
                WindFarm emptyFarm = new WindFarm{ Turbines = Array.Empty<Turbine>()}; 
                var validated = validator.ValidateOrThrow(emptyFarm);

                Console.WriteLine($"Average turbine height in empty windfarm: {calculation.AverageTurbineHeight(validated)}m");
            }
            catch (FluentValidation.ValidationException e)
            {
                PrintValidationException(e);
            }
            
            void PrintValidationException(FluentValidation.ValidationException e)
            {
                Console.WriteLine("Validation failed");
                foreach (var error in e.Errors)
                {
                    Console.WriteLine($" - {error.ErrorMessage}");
                }
            }
        }
    }
}
