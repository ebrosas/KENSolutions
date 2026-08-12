using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>Base request carrying search, sort and paging state from the data grids.</summary>
namespace KenHRApp.Application.Common.Models
{
    public abstract class PagedQuery
    {
        private int _pageSize = 25;
        private int _page = 1;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value is < 1 or > 200 ? 25 : value;
        }

        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }

        public int Skip => (Page - 1) * PageSize;
    }
}
