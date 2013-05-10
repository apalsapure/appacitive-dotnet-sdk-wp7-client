using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Appacitive.Mobile.Windows.Todo.Utility;
using Appacitive.Mobile.WindowsPhone7.Todo.ViewModels;
using Newtonsoft.Json.Linq;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    class ListItemManager
    {
        private const string CONNECTION_TYPE = "list_items";
        private const string END_POINT_A = "TodoLists";
        private const string END_POINT_B = "Tasks";

        public async static Task<ListItemViewModel> Save(ListItemViewModel listItem)
        {
            try
            {
                Sdk.Connection user_listItem_Connection = Sdk.Connection
                                                                                    .New(CONNECTION_TYPE)
                                                                                    .FromExistingArticle(END_POINT_A, App.DetailsViewModel.ListId)
                                                                                    .ToNewArticle(END_POINT_B, listItem);
                await user_listItem_Connection.SaveAsync();
                return (await user_listItem_Connection.GetEndpointArticleAsync(END_POINT_B)).ToListItemViewModel();
            }
            catch { return null; }
        }

        public async static Task<ListItemViewModel> Update(ListItemViewModel listItem)
        {
            try
            {
                await listItem.SaveAsync();
                return listItem;
            }
            catch { return null; }

        }

        public async static Task<List<ListItemViewModel>> Find(ListViewModel list)
        {
            var listItems = new List<ListItemViewModel>();

            var response = await list.GetConnectedArticlesAsync(CONNECTION_TYPE, pageSize: Constants.PAGE_SIZE);
            while (response.Count > 0)
            {
                response.ForEach(r => listItems.Add(r.ToListItemViewModel()));
                if (response.IsLastPage) break;
                await response.NextPageAsync();
            }
            return listItems;
        }

        public async static Task<bool> Delete(ListItemViewModel list, bool deleteConnection = false)
        {
            try
            {
                await Sdk.Articles.DeleteAsync(list.Type, list.Id, deleteConnection);
                return true;
            }
            catch { return false; }
        }

        public async static Task<bool> Delete(List<ListItemViewModel> listItemIDs)
        {
            if (listItemIDs == null || listItemIDs.Count == 0) return true;
            try
            {
                await Sdk.Articles.MultiDeleteAsync(ListItemViewModel.TYPE, listItemIDs.Select(s => s.Id).ToArray());

                return true;
            }
            catch { return false; }
        }
    }
}
