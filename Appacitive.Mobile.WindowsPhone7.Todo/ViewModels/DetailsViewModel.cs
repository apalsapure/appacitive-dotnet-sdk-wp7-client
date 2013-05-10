using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using Appacitive.Mobile.Windows.Todo.Utility;
using Appacitive.Mobile.WindowsPhone7.Todo;
using System.Threading.Tasks;

namespace Appacitive.Mobile.WindowsPhone7.Todo.ViewModels
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        public DetailsViewModel()
        {
            this.Items = new ObservableCollection<ListItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ListItemViewModel> Items { get; private set; }

        private string _listname;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ListName
        {
            get
            {
                return _listname;
            }
            set
            {
                if (value != _listname)
                {
                    _listname = value;
                    NotifyPropertyChanged("ListName");
                }
            }
        }
        public string ListId { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData(ListViewModel list)
        {
            this.ListName = list.Name;
            this.ListId = list.Id;
            this.Items.Clear();

            var lists = await ListItemManager.Find(list);
            foreach (var item in lists.SortByDate())
                this.Items.Add(item);
            this.IsDataLoaded = true;
        }

        public void Add(ListItemViewModel listItem)
        {
            var lists = this.Items.ToList();
            this.Items.Clear();
            lists.Add(listItem);

            foreach (var list in lists.SortByDate())
                this.Items.Add(list);
        }
        public void Remove(ListItemViewModel listItem)
        {
            this.Items.Remove(listItem);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}