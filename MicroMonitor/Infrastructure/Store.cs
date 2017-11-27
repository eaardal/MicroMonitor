using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMonitor.Infrastructure
{
    class Store
    {
        public void Configure(params IReducer[] reducers)
        {
            
        }
    }

    internal interface IState<out T>
    {
        T State { get; }
    }

    internal interface IReducer
    {
    }

    internal interface IRequestHandlerWithStore<IState<out T>>
    {
        
    }
}
