using System;
using System.Threading.Tasks;
using MediatR;
using MicroMonitor.Config;
using MicroMonitor.Services;

namespace MicroMonitor.Actions
{
    class EventLogActionsHandler : IAsyncNotificationHandler<RefreshEventLogEntries>, INotificationHandler<StartPollingForEventLogEntries>
    {
        private readonly IEventLogPollingCoordinator _eventLogPoller;
        private readonly IMediator _mediator;
        private readonly IEventLogService _eventLogService;

        public EventLogActionsHandler(IMediator mediator, IEventLogService eventLogService, IEventLogPollingCoordinator eventLogPoller)
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

        public void Handle(StartPollingForEventLogEntries notification)
        {
            var logName = AppConfiguration.LogName();
            var pollIntervalSeconds = AppConfiguration.PollIntervalSeconds();
            
            _eventLogPoller.Start(logName, pollIntervalSeconds);
        }
    }
}
