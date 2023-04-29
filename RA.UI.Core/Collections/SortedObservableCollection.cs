using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Collections
{
    public class SortedObservableCollection<T> : ObservableCollection<T>
    {
        private readonly IComparer<T> comparer;

        public SortedObservableCollection(IComparer<T> comparer = null)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        protected override void InsertItem(int index, T item)
        {
            int i = 0;
            while (i < Count && comparer.Compare(item, this[i]) > 0)
            {
                i++;
            }
            base.InsertItem(i, item);
        }
    }
}
