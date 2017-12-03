using System;
using MicroMonitor.Infrastructure;

namespace MicroMonitor.Actions
{
    class DetailsWindowActionsHandler
    {
        private readonly IAppStore _store;

        public DetailsWindowActionsHandler(IAppStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }
    }
}
