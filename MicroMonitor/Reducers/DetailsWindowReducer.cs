using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MicroMonitor.Actions;

namespace MicroMonitor.Reducers
{
    class DetailsWindowReducer : INotificationHandler<CreatedNewDetailsWindow>
    {
        private readonly DetailsWindowState _state;

        public DetailsWindowReducer()
        {
            _state = new DetailsWindowState();
        }
        
        public void Handle(CreatedNewDetailsWindow message)
        {
            _state.OpenDetailsWindows.Add(message.NewDetailsWindow);
        }
    }
}
