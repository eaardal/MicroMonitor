using System;
using System.Threading.Tasks;
using System.Windows;
using MediatR;
using MicroMonitor.Config;
using MicroMonitor.Services;
using MicroMonitor.Services.EventLog;

namespace MicroMonitor.Actions
{
    class EventLogActionsHandler : IAsyncNotificationHandler<RefreshEventLogEntries>, IAsyncNotificationHandler<StartPollingForEventLogEntries>
    {
        private readonly IEventLogPoller _eventLogPoller;
        private readonly IMediator _mediator;
        private readonly IEventLogService _eventLogService;

        public EventLogActionsHandler(IMediator mediator, IEventLogService eventLogService, IEventLogPoller eventLogPoller)
        {
            _eventLogPoller = eventLogPoller ?? throw new ArgumentNullException(nameof(eventLogPoller));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _eventLogService = eventLogService ?? throw new ArgumentNullException(nameof(eventLogService));
        }

        public async Task Handle(RefreshEventLogEntries message)
        {
            await _mediator.Publish(new RefreshEventLogEntriesStart());

            var logName = AppConfiguration.LogName();

            try
            {
                var eventLogEntries = await _eventLogService.GetEventLogEntries(logName);

                await _mediator.Publish(new RefreshEventLogEntriesSuccess(logName, eventLogEntries));
            }
            catch (Exception e)
            {
                await _mediator.Publish(new RefreshEventLogEntriesError(e));
            }
        }

        public async Task Handle(StartPollingForEventLogEntries notification)
        {
            var logName = AppConfiguration.LogName();
            var pollIntervalSeconds = AppConfiguration.PollIntervalSeconds();
            
            StartPollingForEventViewerLogs(logName, pollIntervalSeconds);
        }

        private void StartPollingForEventViewerLogs(string logName, int pollIntervalSeconds)
        {
            _eventLogPoller.StartPollingAtIntervals(logName, pollIntervalSeconds);

            var readInterval = pollIntervalSeconds + 2;

            _expectedNextReadTime = DateTime.Now.AddSeconds(readInterval);

            _nextReadTimer.Interval = 1000;
            _nextReadTimer.Elapsed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_expectedNextReadTime != DateTime.MinValue)
                    {
                        var nextReadInSeconds = _expectedNextReadTime.Subtract(DateTime.Now).Seconds;
                        Model.NextReadText = $"Next read: {nextReadInSeconds}s";
                    }
                    else
                    {
                        Model.NextReadText = "Next read: TBD";
                    }

                });
            };
            _nextReadTimer.Start();

            _microLogReader.ReadOnInterval(logName, readInterval, newLogEntries =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _lastReadTime = DateTime.Now;
                    _expectedNextReadTime = _lastReadTime.AddSeconds(readInterval);

                    GroupAndBindLogEntries(newLogEntries.ToArray());
                    UpdateLastRead();
                });
            });
        }
    }
}
