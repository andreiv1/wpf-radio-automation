using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject, IDisposable
    {
        public virtual void Dispose() { }
    }
}
