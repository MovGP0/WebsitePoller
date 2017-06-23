using AutoMapper;
using NodaTime;
using WebsitePoller.Entities;

namespace WebsitePoller.Mappings
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, LocalTime>().ConvertUsing<LocalTimeConverter>();
            CreateMap<SettingsStrings, Settings>();
        }
    }
}