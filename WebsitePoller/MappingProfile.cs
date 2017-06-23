using AutoMapper;
using NodaTime;
using WebsitePoller.Setting;

namespace WebsitePoller
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