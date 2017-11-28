using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
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
