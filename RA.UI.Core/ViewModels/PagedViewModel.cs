using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.ViewModels
{
    /// <summary>
    /// Abstract view model responsible for handling pages of content (views)
    /// </summary>
    public abstract partial class PagedViewModel<TViewModel> : ViewModelBase
        where TViewModel : ViewModelBase
    {
        public delegate void PageChangedEventHandler(object sender, int newPageIndex);

        public event PageChangedEventHandler PageChanged;

        [ObservableProperty]
        private INavigationService<TViewModel> navigationService;
        protected List<ViewModelBase> viewModels;


        private int pageIndex = 0;

        /// <summary>
        /// Change the page with one associated with the view models collection
        /// Actually, it is the index from the view models collection
        /// </summary>
        public int Page
        {
            get => pageIndex;
            set
            {
                if (value >= 0 && value < viewModels.Count)
                {
                    SetProperty(ref pageIndex, value);
                    if (viewModels.Count > 0)
                    {
                        ViewModelBase viewModel = viewModels[value];
                        NavigationService.NavigateTo(viewModel);
                        PageChanged?.Invoke(this, value);
                        GoToNextPageCommand.NotifyCanExecuteChanged();
                        GoToPreviousPageCommand.NotifyCanExecuteChanged();
                    }
                    else throw new InvalidOperationException("Cannot navigate to a page when there are no view models available.");
                }
                else throw new IndexOutOfRangeException($"Page with index {value} does not exist. Page count is {PageCount}");

            }
        }

        public int PageCount { get => viewModels.Count; }

        protected PagedViewModel(INavigationService<TViewModel> navigationService)
        {
            viewModels = new List<ViewModelBase>();
            this.navigationService = navigationService;

            InitialisePages();
            Page = 0;
        }

        protected abstract void InitialisePages();

        [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
        private void GoToNextPage()
        {
            if (Page < PageCount - 1)
            {
                Page++;
            }
        }

        private bool CanGoToNextPage()
        {
            return Page < viewModels.Count - 1;
        }

        [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
        private void GoToPreviousPage()
        {
            if (Page > 0)
            {
                Page--;
            }
        }

        protected virtual bool CanGoToPreviousPage()
        {
            return Page > 0;
        }
    }
}
