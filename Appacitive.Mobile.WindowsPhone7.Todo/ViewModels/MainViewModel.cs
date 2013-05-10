using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using Appacitive.Mobile.WindowsPhone7.Todo;
using Appacitive.Mobile.Windows.Todo.Utility;
using System.Threading.Tasks;

namespace Appacitive.Mobile.WindowsPhone7.Todo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ListViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ListViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData()
        {

            var lists = await ListManager.Find();
            this.Items.Clear();

            foreach (var list in lists.SortByDate())
            {
                this.Items.Add(list);
            }
            this.IsDataLoaded = true;
        }

        public void Add(ListViewModel listItem)
        {
            var lists = this.Items.ToList();
            this.Items.Clear();
            lists.Add(listItem);

            foreach (var list in lists.SortByDate())
                this.Items.Add(list);
        }

        public void Remove(ListViewModel listItem)
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