namespace DataModel
{
    using AutoMapper;
    using Validated = DataModel.Dto.Validated;
    
    internal class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<Dto.Location, Validated.Location>();
            CreateMap<Dto.Turbine, Validated.Turbine>();
            CreateMap<Dto.WindFarm, Validated.WindFarm>();
        }
    }
}