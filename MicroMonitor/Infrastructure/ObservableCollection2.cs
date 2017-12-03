using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMonitor.Infrastructure
{
    class ObservableCollection2<T> : ObservableCollection<T>
    {
        public void InsertItem2(int position, T item)
        {
            base.InsertItem(position, item);
            //base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, ));
        }
    }
}
