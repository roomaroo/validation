using System.Collections.Generic;

namespace DataModel.Dto
{
    public class WindFarm
    {
        public IList<Turbine> Turbines { get; set; }
        public Location Location { get; set; }
    }
}