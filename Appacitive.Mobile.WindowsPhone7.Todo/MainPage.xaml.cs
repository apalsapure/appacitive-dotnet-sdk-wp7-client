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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Appacitive.Mobile.WindowsPhone7.Todo.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

        }

        // Handle selection changed on LongListSelector
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;
            // If selected item is null (no selection) do nothing
            if (MainListBox.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainListBox.SelectedItem as ListViewModel).Id, UriKind.Relative));

            // Reset selected item to null (no selection)
            MainListBox.SelectedItem = null;
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void lnkSignUp_Click(object sender, RoutedEventArgs e)
        {
            gSingIn.Visibility = System.Windows.Visibility.Collapsed;
            gSingUp.Visibility = System.Windows.Visibility.Visible;
        }

        private void lnkSignIn_Click(object sender, RoutedEventArgs e)
        {
            gSingIn.Visibility = System.Windows.Visibility.Visible;
            gSingUp.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            btnSignIn.IsEnabled = false;
            progress.Visibility = System.Windows.Visibility.Visible;

            var isSuccess = await UserManager.Authenticate(txtEmail.Text, txtPassword.Password);

            if (isSuccess == false)
            {
                MessageBox.Show("Oops please check your email address and password", "Sign in Failed", MessageBoxButton.OK);
                progress.Visibility = System.Windows.Visibility.Collapsed;
                btnSignIn.IsEnabled = true;
            }
            else
            {
                ShowList();
                btnSignIn.IsEnabled = true;
            }
        }

        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            btnSignUp.IsEnabled = false;
            progress.Visibility = System.Windows.Visibility.Visible;

            //Get first name & last name
            var split = txtRName.Text.Split(' ');
            string lastName = string.Empty;
            if (split.Length > 1) lastName = string.Join(" ", split.Skip(1));

            var user = new Sdk.User
            {
                Email = txtREmail.Text,
                FirstName = split[0],
                LastName = lastName,
                Username = txtREmail.Text
            };

            Context.User = await UserManager.Save(user, txtRPassword.Password);

            if (Context.User == null)
            {
                MessageBox.Show("Oops some thing went wrong, check your network connection.", "Sign up failed", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Login with your email address and password.", "Sign up successful", MessageBoxButton.OK);
                txtEmail.Text = txtREmail.Text;
                txtREmail.Text = txtRName.Text = txtRPassword.Password = string.Empty;
                lnkSignIn_Click(null, null);
            }
            progress.Visibility = System.Windows.Visibility.Collapsed;
            btnSignUp.IsEnabled = true;
        }

        private async void ShowList()
        {
            if (gSingIn.Visibility == System.Windows.Visibility.Visible) gSingIn.Visibility = System.Windows.Visibility.Collapsed;
            if (gSingUp.Visibility == System.Windows.Visibility.Visible) gSingUp.Visibility = System.Windows.Visibility.Collapsed;
            if (gTodoList.Visibility == System.Windows.Visibility.Collapsed) gTodoList.Visibility = System.Windows.Visibility.Visible;

            if (!App.ViewModel.IsDataLoaded)
            {
                await App.ViewModel.LoadData();
            }
            ApplicationBar.IsVisible = true;
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void appBarAdd_Click(object sender, EventArgs e)
        {
            gTodoList.Visibility = System.Windows.Visibility.Collapsed;
            gAddList.Visibility = System.Windows.Visibility.Visible;

            txtListName.Focus();
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
            progress.Visibility = System.Windows.Visibility.Visible;
            if (string.IsNullOrWhiteSpace(txtListName.Text) == false)
            {
                (sender as ApplicationBarIconButton).IsEnabled = false;
                var list = await ListManager.Save(new ListViewModel(txtListName.Text.Trim()));
                if (list == null)
                {
                    MessageBox.Show("Failed to save the list", "Operation failed", MessageBoxButton.OK);
                }
                else
                {
                    txtListName.Text = string.Empty;
                    App.ViewModel.Add(list);
                }
            }
            (sender as ApplicationBarIconButton).IsEnabled = true;
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
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;
            var list = App.ViewModel.Items.Where(i => i.Id.Equals(((Microsoft.Phone.Controls.MenuItem)(sender)).CommandParameter)).FirstOrDefault();
            if (list == null) return;
            var isSuccess = await ListManager.Delete(list, true);
            progress.Visibility = System.Windows.Visibility.Collapsed;
            if (isSuccess == false)
            {
                MessageBox.Show("Failed to delete the list.", "Operation failed", MessageBoxButton.OK);
            }
            else
            {
                App.ViewModel.Remove(list);
            }
        }
        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    }
}