using System.IO;
using System.Reflection;
using AutoMapper;
using Hjson;
using JetBrains.Annotations;
using Newtonsoft.Json;
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

        public Settings Load([NotNull]string path)
        {
            var jsonString = HjsonValue.Load(path).ToString();
            return DeserializeSettings(jsonString);
        }

        private Settings DeserializeSettings(string jsonString)
        {
            var result = JsonConvert.DeserializeObject<SettingsStrings>(jsonString);
            return Mapper.Map<SettingsStrings, Settings>(result);
        }
    }
}