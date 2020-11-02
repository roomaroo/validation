using System.Collections.Generic;

namespace Calculation.Validated
{
    public class WindFarm
    {
        public IList<Turbine> Turbines { get; }
        public Location Location { get; }

        internal WindFarm(IList<Turbine> turbines, Location location)
        {
            Turbines = turbines;
            Location = location;
        }
    }
}