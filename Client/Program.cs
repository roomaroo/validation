using System;
using Calculation;
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
            Console.WriteLine($"Average turbine height: {calculation.AverageTurbineHeight(windFarm)}m");

            try
            {
                WindFarm emptyFarm = new WindFarm(); 
                // Throws ArgumentNull because the turbine list is null
                Console.WriteLine($"Average turbine height in empty windfarm: {calculation.AverageTurbineHeight(emptyFarm)}m");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught {e.GetType().Name}");
            }

            try
            {
                WindFarm emptyFarm = new WindFarm{ Turbines = Array.Empty<Turbine>()}; 
                // Returns NaN because the turbine list is empty
                Console.WriteLine($"Average turbine height in empty windfarm: {calculation.AverageTurbineHeight(emptyFarm)}m");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught {e.GetType().Name}");
            }
        }
    }
}
