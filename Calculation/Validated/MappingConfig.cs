namespace Calculation.Validated
{
    using AutoMapper;
    using Dto = DataModel.Dto;
    
    internal class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<Dto.Location, Location>();
            CreateMap<Dto.Turbine, Turbine>();
            CreateMap<Dto.WindFarm, WindFarm>();
        }
    }
}