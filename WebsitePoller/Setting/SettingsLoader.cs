using System;
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
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<SettingsLoader>();

        [NotNull]
        private SettingsManager SettingsManager { get; }

        [NotNull]
        private IMapper Mapper { get; }

        public SettingsLoader([NotNull] SettingsManager settingsManager, [NotNull] IMapper mapper)
        {
            SettingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void UpdateSettings()
        {
            var path = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "settings.hjson");
            if (!File.Exists(path))
            {
                Log.Error($"Could not find file '{path}'");
                return;
            }

            var settings = Load(path);
            SettingsManager.Settings = settings;
        }

        public Settings Load([NotNull] string path)
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