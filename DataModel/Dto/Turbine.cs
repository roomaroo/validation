using System;

namespace DataModel.Dto
{
    public class Turbine
    {
        public Guid Id { get; set; }
        public double TowerHeight { get; set; }

        public double BladeLength { get; set; }

        public Location Location { get; set; }
    }
}
