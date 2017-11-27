using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicroMonitor.Reducers
{
    class DetailsWindowState
    {
        public List<Window> OpenDetailsWindows { get; set; } = new List<Window>();
    }
}
