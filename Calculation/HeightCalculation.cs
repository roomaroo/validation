using System.Linq;

namespace Calculation
{
    public class HeightCalculation
    {
        public double AverageTurbineHeight(Validated.WindFarm windFarm)
        {
            // No need to validate the inputs. It's a Validated.WindFarm, which
            // can only be created by going through validation
            double totalHeight = windFarm.Turbines.Sum(t => t.TowerHeight + t.BladeLength);
            return totalHeight / windFarm.Turbines.Count; 
        }
    }
}
