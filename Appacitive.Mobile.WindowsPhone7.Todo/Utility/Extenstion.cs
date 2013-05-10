using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using Appacitive.Mobile.WindowsPhone7.Todo.ViewModels;
using System.Threading.Tasks;

namespace Appacitive.Mobile.Windows.Todo.Utility
{
    public static class CustomExtensions
    {
        public static ListViewModel ToListViewModel(this Sdk.Article article)
        {
            return new ListViewModel(article);
        }
        public static ListItemViewModel ToListItemViewModel(this Sdk.Article article)
        {
            return new ListItemViewModel(article);
        }

        public static IEnumerable<ListItemViewModel> SortByDate(this IEnumerable<ListItemViewModel> items)
        {
            return items.OrderByDescending(i => i.UtcCreateDate);
        }
        public static IEnumerable<ListViewModel> SortByDate(this IEnumerable<ListViewModel> items)
        {
            return items.OrderByDescending(i => i.UtcCreateDate);
        }
    }
}
