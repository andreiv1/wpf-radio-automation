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
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                //TODO
                if (disposing)
                {
                    // Release managed resources here
                }

                // Release unmanaged resources here

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }
    }
}
