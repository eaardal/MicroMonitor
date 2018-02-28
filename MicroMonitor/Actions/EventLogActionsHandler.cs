using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroMonitor.Config;
using MicroMonitor.Services;

namespace MicroMonitor.Actions
{
    internal class EventLogActionsHandler : INotificationHandler<RefreshEventLogEntries>, INotificationHandler<StartPollingForEventLogEntries>
    {
        private readonly IEventLogReader _eventLogReader;
        private readonly IConfiguration _configuration;
        private readonly IEventLogPollingCoordinator _eventLogPoller;
        private readonly IMediator _mediator;

        public EventLogActionsHandler(IMediator mediator, IEventLogReader eventLogReader, IEventLogPollingCoordinator eventLogPoller, IConfiguration configuration)
        {
            _eventLogReader = eventLogReader ?? throw new ArgumentNullException(nameof(eventLogReader));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _eventLogPoller = eventLogPoller ?? throw new ArgumentNullException(nameof(eventLogPoller));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(RefreshEventLogEntries message, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new RefreshEventLogEntriesStart(), cancellationToken);

            var logName = _configuration.LogName();

            try
            {
                var eventLogEntries = await _eventLogReader.ReadEventLogAsync(logName);

                await _mediator.Publish(new RefreshEventLogEntriesSuccess(logName, eventLogEntries), cancellationToken);
            }
            catch (Exception e)
            {
                await _mediator.Publish(new RefreshEventLogEntriesError(e), cancellationToken);
            }
        }

        public async Task Handle(StartPollingForEventLogEntries notification, CancellationToken cancellationToken)
        {
            var logName = _configuration.LogName();
            var pollIntervalSeconds = _configuration.PollIntervalSeconds();
            
            await Task.Run(() => _eventLogPoller.Start(logName, pollIntervalSeconds), cancellationToken);
        }
    }
}
