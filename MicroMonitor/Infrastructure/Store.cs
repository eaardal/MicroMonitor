using System;
using System.Threading.Tasks;
using MediatR;

namespace MicroMonitor.Infrastructure
{
    public class Store<TState> : IStore<TState> where TState : class, new()
    {
        private readonly IMediator _mediator;
        private readonly TState _state;

        public Store(IMediator mediator, TState initialState)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _state = initialState ?? throw new ArgumentNullException(nameof(initialState));
        }

        public TState GetState()
        {
            return _state;
        }

        public async Task Dispatch<TMessage>(TMessage message) where TMessage : INotification
        {
            await _mediator.Publish(message);
        }
    }
}
