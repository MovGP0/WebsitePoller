using AutoMapper;
using DryIoc;

namespace WebsitePoller.Mappings
{
    public static class RegistratorExtensions
    {
        public static IRegistrator SetupMappings(this IRegistrator registrator)
        {
            registrator.RegisterDelegate(r => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }));
            registrator.RegisterDelegate<IMapper>(r => new Mapper(r.Resolve<MapperConfiguration>()), Reuse.Singleton);
            return registrator;
        }
    }
}
