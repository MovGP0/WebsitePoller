using System.IO;
using System.Reflection;
using AutoMapper;
using Hjson;
using Jil;
using Serilog;
using WebsitePoller.Entities;
using WebsitePoller.Workflow;

namespace WebsitePoller.Setting
{
    public sealed class SettingsLoader : ISettingsLoader
    {
        private static ILogger Log => Serilog.Log.ForContext<SettingsLoader>();
        private SettingsManager SettingsManager { get; }
        private IMapper Mapper { get; }

        public SettingsLoader(SettingsManager settingsManager, IMapper mapper)
        {
            SettingsManager = settingsManager;
            Mapper = mapper;
        }
        
        public void UpdateSettings()
        {
            var path = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "settings.hjson");
            if (!File.Exists(path))
            {
                Log.Warning($"Could not open file '{path}'");
                return;
            }

            var settings = Load(path);
            SettingsManager.Settings = settings;
        }

        public Settings Load(string path)
        {
            var jsonString = HjsonValue.Load(path).ToString();
            return DeserializeSettings(jsonString);
        }

        private Settings DeserializeSettings(string jsonString)
        {
            using (var input = new StringReader(jsonString))
            {
                var result = JSON.Deserialize<SettingsStrings>(input);
                return Mapper.Map<SettingsStrings, Settings>(result);
            }
        }
    }
}