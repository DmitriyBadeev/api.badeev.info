using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Extensions.Logging;
using Portfolio.Finance.API.Services.Interfaces;

namespace Portfolio.Finance.API.Services
{
    public class TimerService : ITimerService
    {
        private readonly ILogger<TimerService> _logger;
        private Timer _timer = new Timer(10000);

        private readonly Dictionary<string, ElapsedEventHandler> _handlers = new Dictionary<string, ElapsedEventHandler>();
        private readonly Timer _reloadTimer = new Timer(36000000);

        public TimerService(ILogger<TimerService> _logger)
        {
            this._logger = _logger;
            _logger.LogInformation("Start timer service");
            _timer.AutoReset = true;
            _timer.Start();

            _reloadTimer.AutoReset = true;
            _reloadTimer.Start();
            _reloadTimer.Elapsed += (source, args) =>
            {
                _logger.LogInformation("Clear all events");
                _timer.Dispose();
                _handlers.Clear();
                _timer = new Timer(10000) { AutoReset = true };
                _timer.Start();
            };
        }

        public string Subscribe(ElapsedEventHandler handler)
        {
            _timer.Elapsed += handler;

            var guid = Guid.NewGuid().ToString();
            _logger.LogInformation($"Subscribe event {guid}");
            _handlers.Add(guid, handler);

            return guid;
        }

        public void Unsubscribe(string handlerId)
        {
            _logger.LogInformation($"Unsubscribe event {handlerId}");
            var handler = _handlers[handlerId];

            _timer.Elapsed -= handler;
        }

        public void Reload()
        {
            _logger.LogInformation("Clear all events");
            _timer.Dispose();

            _timer = new Timer(20000) { AutoReset = true };
            _timer.Start();
        }
    }
}
