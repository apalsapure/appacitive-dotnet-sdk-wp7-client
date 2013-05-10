using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Appacitive.Mobile.Windows.Todo.Utility;
using Appacitive.Mobile.WindowsPhone7.Todo.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    public partial class Settings : PhoneApplicationPage
    {
        public ImageDataContext ImageDataContext { get; set; }
        public Settings()
        {
            InitializeComponent();
            ImageDataContext = new ImageDataContext();
            imageProfile.ImageFailed += imageProfile_ImageFailed;
        }

        void imageProfile_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageDataContext.Image = Constants.DEFAULT_PHOTO_ADD;
            imageProfile.Visibility = System.Windows.Visibility.Visible;
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Context.User["image"]) == false) imageProfile.Visibility = System.Windows.Visibility.Collapsed;
            progress.Visibility = System.Windows.Visibility.Visible;

            Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Context.User["image"]) == false)
                    {
                        var image = new BitmapImage();

                        //**************** APPACITIVE HELP TEXT **************//
                        //Following code demos two implementations of download file
                        // 1) Download file as byte data
                        // 2) Download file to local storage

                        var downloadHandler = new Sdk.FileDownload(Context.User["image"]);

                        //Case 1 : Download the file as bytes
                        var bytes = await downloadHandler.DownloadAsync();
                        image.SetSource(new MemoryStream(bytes));
                        imageProfile.Source = image;

                        //Case 2 : Download the file to local store
                        //string imagePath = "profileImage.jpg";
                        //await downloadHandler.DownloadFileAsync(imagePath);
                        //ImageDataContext.Image = imagePath;
                    }
                    else
                    {
                        imageProfile.DataContext = ImageDataContext;
                        ImageDataContext.Image = Context.User["image"];
                    }
                    progress.Visibility = System.Windows.Visibility.Collapsed;
                    imageProfile.Visibility = System.Windows.Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        private void imageProfile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MediaUpload.xaml", UriKind.Relative));
        }
    }
}