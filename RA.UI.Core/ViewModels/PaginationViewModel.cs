using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Logic;
using RA.UI.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core.ViewModels
{
    /// <summary>
    /// Abstract view model used to paginate a collection
    /// </summary>
    /// <typeparam name="T">Type of the paginated collection</typeparam>
    public abstract partial class PaginationViewModel<T> : ViewModelBase
    {
        [ObservableProperty]
        private PaginationHelper<T> paginationHelper;

        /// <summary>
        /// Items which will be paginated
        /// </summary>
        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();
        public int RecordsPerPage { get; set; } = 50;

        protected PaginationViewModel()
        {
            PaginationHelper = new PaginationHelper<T>(RecordsPerPage);
        }

        protected async Task LoadItemsAsync(bool isFirstPage = false)
        {
            throw new NotImplementedException();
            //    using (var db = new AppDbContext())
            //    {
            //        var query = GetItemsQuery(db);

            //        var count = await query.CountAsync();

            //        DebugHelper.WriteLine(this, $"Total items count: {count}, records per page: {RecordsPerPage}");

            //        PaginationHelper.UpdatePagination(count);

            //        var dtoQuery = ApplyPagination(query);

            //        var dtos = await dtoQuery.ToListAsync();

            //        Items.Clear(); 

            //        foreach (var dto in dtos)
            //        {
            //            Items.Add(dto);
            //        }

            //        if (isFirstPage)
            //        {
            //            PaginationHelper.CurrentRecords = Items.Count;
            //        }

            //        // Notify the UI that the previous/next page commands can be executed
            //        Application.Current.Dispatcher.Invoke(() =>
            //        {
            //            PreviousPageCommand.NotifyCanExecuteChanged();
            //            NextPageCommand.NotifyCanExecuteChanged();
            //        });
            //    }
        }


        protected abstract IQueryable<T> GetItemsQuery(AppDbContext db);

        protected virtual IQueryable<T> ApplyPagination(IQueryable<T> query)
        {
            return query
                .Skip(PaginationHelper.Offset)
                .Take(PaginationHelper.RecordsPerPage);
        }

        #region Pagination commands

        [RelayCommand(CanExecute = nameof(CanGoNextPage))]
        private async void NextPage()
        {
            PaginationHelper.NextPage();
            await LoadItemsAsync();
            PaginationHelper.CurrentRecords += Items.Count;
        }

        private bool CanGoNextPage()
        {
            return PaginationHelper.CanGoNextPage();
        }

        [RelayCommand(CanExecute = nameof(CanGoPreviousPage))]
        private async void PreviousPage()
        {
            PaginationHelper.PreviousPage();
            PaginationHelper.CurrentRecords -= Items.Count;
            await LoadItemsAsync();

            int remainder = PaginationHelper.CurrentRecords % PaginationHelper.RecordsPerPage;
            if (remainder > 0)
            {
                PaginationHelper.Offset -= PaginationHelper.RecordsPerPage - remainder;
            }
        }

        private bool CanGoPreviousPage()
        {
            return PaginationHelper.CanGoPreviousPage();
        }
        #endregion
    }

}
