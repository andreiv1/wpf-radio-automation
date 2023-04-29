using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Helpers 
{
    public partial class PaginationHelper<T> : ObservableObject
    {
        private int totalPages;
        private int totalRecords;
        private int pageNo = 1;
        private int currentRecords;

        public int TotalPages
        {
            get => totalPages;
            private set => SetProperty(ref totalPages, value);
        }

        public int TotalRecords
        {
            get => totalRecords;
            private set => SetProperty(ref totalRecords, value);
        }

        public int CurrentRecords
        {
            get => currentRecords;
            set => SetProperty(ref currentRecords, value);
        }
        public int RecordsPerPage { get; set; }
        public int Offset { get; set; }

        public int PageNo
        {
            get => pageNo;
            set
            {
                if (value > 0 && value <= TotalPages)
                {
                    SetProperty(ref pageNo, value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Index {value} out of range.");
                }
            }
        }

        public PaginationHelper(int recordsPerPage = 50)
        {
            RecordsPerPage = recordsPerPage;
        }

        public void UpdatePagination(int totalRecords)
        {
            TotalRecords = totalRecords;
            int remainder = totalRecords % RecordsPerPage;
            TotalPages = totalRecords / RecordsPerPage;
            if (remainder > 0)
            {
                TotalPages += 1;
            }
        }

        public IQueryable<T> ApplyPagination(IQueryable<T> query)
        {
            return query.Skip(Offset).Take(RecordsPerPage);
        }
        public void NextPage()
        {
            PageNo++;
            Offset += RecordsPerPage;
        }

        public void PreviousPage()
        {
            PageNo--;
            Offset -= RecordsPerPage;
        }

        public bool CanGoNextPage()
        {
            return PageNo < TotalPages;
        }

        public bool CanGoPreviousPage()
        {
            return PageNo > 1;
        }
    }

}
