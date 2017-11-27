using System;
using System.Threading.Tasks;
using MediatR;
using MicroMonitor.Config;
using MicroMonitor.Services;

namespace MicroMonitor.Actions
{
    class EventLogActionsHandler : IAsyncNotificationHandler<RefreshEventLogEntries>
    {
        private readonly IMediator _mediator;
        private readonly IEventLogService _eventLogService;

        public EventLogActionsHandler(IMediator mediator, IEventLogService eventLogService)
        {
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
    }
}
