using System;
using System.Linq;
using DataModel.Dto;
using DataModel.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace Calculation
{
    public class HeightCalculation
    {
        public double AverageTurbineHeight(WindFarm windFarm)
        {
            // Validate the inputs. Need to do this in the calculation because we
            // can't assume that the caller has validated the inputs.
            var validator = new WindFarmValidator();
            validator.ValidateAndThrow(windFarm);
            
            double totalHeight = windFarm.Turbines.Sum(t => t.TowerHeight + t.BladeLength);
            return totalHeight / windFarm.Turbines.Count; 
        }
    }
}
