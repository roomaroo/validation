using System;
using System.Linq;
using DataModel.Dto;

namespace Calculation
{
    public class HeightCalculation
    {
        public double AverageTurbineHeight(WindFarm windFarm)
        {
            double totalHeight = windFarm.Turbines.Sum(t => t.TowerHeight + t.BladeLength);
            return totalHeight / windFarm.Turbines.Count; 
        }
    }
}
