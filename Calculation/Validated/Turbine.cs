using System;

namespace Calculation.Validated
{
    public class Turbine
    {
        public Guid Id { get; }
        public double TowerHeight { get; }

        public double BladeLength { get; }

        public Location Location { get; }

        internal Turbine(Guid id, double towerHeight, double bladeLength, Location location)
        {
            Id = id;
            TowerHeight = towerHeight;
            BladeLength = bladeLength;
            Location = location;
        }
    }
}
