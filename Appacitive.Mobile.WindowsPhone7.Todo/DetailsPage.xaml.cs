using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Appacitive.Mobile.WindowsPhone7.Todo.ViewModels;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            DataContext = App.DetailsViewModel;
        }

        // When page is navigated to set data context to selected item in list
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                await App.DetailsViewModel.LoadData(App.ViewModel.Items.Where(i => i.Id == selectedIndex).FirstOrDefault());
            }
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;
            var selectedItem = ((System.Windows.Controls.Primitives.ToggleButton)(sender));
            var selectedListItem = App.DetailsViewModel.Items.Where(i => i.Name == selectedItem.Content.ToString()).FirstOrDefault();
            if (selectedListItem == null) return;
            selectedListItem.IsDone = selectedItem.IsChecked == true;
            var listItem = await ListItemManager.Update(selectedListItem);
            progress.Visibility = System.Windows.Visibility.Collapsed;
            if (listItem == null)
            {
                MessageBox.Show("Failed to save the state of item.", "Operation failed", MessageBoxButton.OK);
            }
            else
            {
                selectedItem.IsChecked = listItem.IsDone;
            }
        }

        private void appBarAdd_Click(object sender, EventArgs e)
        {
            gTodoList.Visibility = System.Windows.Visibility.Collapsed;
            gAddList.Visibility = System.Windows.Visibility.Visible;

            txtListItemName.Focus();
            ApplicationBar.Buttons.RemoveAt(0);
            ApplicationBarIconButton appBarSaveButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            appBarSaveButton.Text = "save";
            appBarSaveButton.Click += appBarSave_Click;
            ApplicationBar.Buttons.Add(appBarSaveButton);

            ApplicationBarIconButton appBarCancelButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/cancel.png", UriKind.Relative));
            appBarCancelButton.Text = "cancel";
            appBarCancelButton.Click += appBarCancel_Click;
            ApplicationBar.Buttons.Add(appBarCancelButton);
        }
        private async void appBarSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtListItemName.Text) == false)
            {
                progress.Visibility = System.Windows.Visibility.Visible;
                (sender as ApplicationBarIconButton).IsEnabled = false;
                var listItem = await ListItemManager.Save(new ListItemViewModel(txtListItemName.Text.Trim()));
                progress.Visibility = System.Windows.Visibility.Collapsed;
                if (listItem == null)
                {
                    MessageBox.Show("Failed to save the todo item", "Operation failed", MessageBoxButton.OK);
                }
                else
                {
                    txtListItemName.Text = string.Empty;
                    App.DetailsViewModel.Add(listItem);
                }
                (sender as ApplicationBarIconButton).IsEnabled = true;
            }
            appBarCancel_Click(null, null);
        }
        private void appBarCancel_Click(object sender, EventArgs e)
        {
            gTodoList.Visibility = System.Windows.Visibility.Visible;
            gAddList.Visibility = System.Windows.Visibility.Collapsed;

            lock (App.DetailsViewModel)
            {
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBar.Buttons.RemoveAt(0);
            }

            ApplicationBarIconButton appBarAddButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarAddButton.Text = "cancel";
            appBarAddButton.Click += appBarAdd_Click;
            ApplicationBar.Buttons.Add(appBarAddButton);
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;
            var listItem = App.DetailsViewModel.Items.Where(i => i.Id.Equals(((Microsoft.Phone.Controls.MenuItem)(sender)).CommandParameter)).FirstOrDefault();
            if (listItem == null) return;
            var isSuccess = await ListItemManager.Delete(listItem, true);
            progress.Visibility = System.Windows.Visibility.Collapsed;
            if (isSuccess == false)
            {
                MessageBox.Show("Failed to delete the list item.", "Operation failed", MessageBoxButton.OK);
            }
            else
            {
                App.DetailsViewModel.Remove(listItem);
            }
        }
    }
}