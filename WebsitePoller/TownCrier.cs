using System;
using System.Timers;
using JetBrains.Annotations;
using NodaTime;
using Serilog;
using WebsitePoller.Setting;

namespace WebsitePoller
{
    public sealed class TownCrier : IDisposable, ITownCrier
    {
        private event EventHandler PrayEvent;
        public event EventHandler Pray
        {
            add => PrayEvent += value;
            remove => PrayEvent -= value;
        }

        private void OnPray()
        {
            PrayEvent?.Invoke(this, EventArgs.Empty);
        }

        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<TownCrier>();

        [NotNull]
        private IIntervallCalculator IntervallCalculator { get; }

        [NotNull]
        private SettingsManager SettingsManager { get; }

        public TownCrier([NotNull]IIntervallCalculator intervallCalculator, [NotNull] SettingsManager settingsManager)
        {
            IntervallCalculator = intervallCalculator;
            SettingsManager = settingsManager;
        }

        ~TownCrier()
        {
            Dispose(false);
        }

        private Timer _timer;

        private void Handle(object sender, ElapsedEventArgs args)
        {
            DisposeTimer();

            try
            {
                OnPray();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            StartTimer();
        }

        private void DisposeTimer()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer.Elapsed -= Handle;
            _timer.Dispose();
        }

        private void StartTimer()
        {
            var timeTillMinTime = IntervallCalculator.CalculateDurationTillIntervall();
            var timeInMilliseconds = SetMinimumDurationWhenZero(timeTillMinTime).TotalMilliseconds;

            _timer = new Timer
            {
                Interval = timeInMilliseconds,
                AutoReset = false
            };
            _timer.Elapsed += Handle;
            _timer.Start();

            Log.Information($"Sleeping for {timeInMilliseconds:0} ms.");
        }

        private Duration SetMinimumDurationWhenZero(Duration timeTillMinTime)
        {
            return timeTillMinTime == Duration.Zero
                ? Duration.FromSeconds(SettingsManager.Settings.PollingIntervallInSeconds)
                : timeTillMinTime;
        }

        public void Start()
        {
            StartTimer();
        }

        public void Stop()
        {
            DisposeTimer();
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (_isDisposed) return;
            Dispose(true);
            _isDisposed = true;
        }

        public void Dispose(bool disposing)
        {
            DisposeTimer();

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}