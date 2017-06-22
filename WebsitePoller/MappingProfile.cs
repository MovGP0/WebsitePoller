using AutoMapper;
using NodaTime;
using WebsitePoller.Settings;

namespace WebsitePoller
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, LocalTime>().ConvertUsing<LocalTimeConverter>();
            CreateMap<SettingsStrings, Settings.Settings>();
        }
    }
}