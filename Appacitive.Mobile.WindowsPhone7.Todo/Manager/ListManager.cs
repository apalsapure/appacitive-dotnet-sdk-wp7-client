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
    class ListManager
    {
        private const string CONNECTION_TYPE = "user_lists";
        private const string END_POINT_A = "user";
        private const string END_POINT_B = "TodoLists";

        public async static Task<ListViewModel> Save(ListViewModel list)
        {
            try
            {
                Sdk.Connection user_list_Connection = Sdk.Connection
                                                                                    .New(CONNECTION_TYPE)
                                                                                    .FromExistingArticle(END_POINT_A, Context.User.Id)
                                                                                    .ToNewArticle(END_POINT_B, list);
                await user_list_Connection.SaveAsync();
                return (await user_list_Connection.GetEndpointArticleAsync(END_POINT_B)).ToListViewModel();
            }
            catch { return null; }
        }

        public async static Task<List<ListViewModel>> Find()
        {
            int pageNumber = 1;
            var lists = new List<ListViewModel>();

            var response = await Context.User.GetConnectedArticlesAsync(CONNECTION_TYPE, null, null, pageNumber++, Constants.PAGE_SIZE);
            while (response.Count > 0)
            {
                response.ForEach(r => lists.Add(r.ToListViewModel()));
                if (response.IsLastPage) break;
                await response.NextPageAsync();
            }
            return lists;
        }

        public async static Task<bool> Delete(ListViewModel list, bool deleteConnection = false)
        {
            try
            {
                //Step 1: Get all the list items for the given list, need to delete them
                var listItems = await ListItemManager.Find(list);

                //Step 2: Delete the list
                await Sdk.Articles.DeleteAsync(list.Type, list.Id, deleteConnection);

                //Step 3: Multidelete the all the list items
                ListItemManager.Delete(listItems);
                return true;
            }
            catch { return false; }
        }
    }
}
