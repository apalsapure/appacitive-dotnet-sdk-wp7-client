using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExifLib;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    public partial class MediaUpload : PhoneApplicationPage
    {
        PhotoChooserTask _photoChooserTask;
        CameraCaptureTask _cameraCaptureTask;
        Stream _capturedImage;
        MemoryStream _imageStream;
        int _width;
        int _height;
        ExifLib.ExifOrientation _orientation;
        int _angle;

        private const string IMAGE_PATH = "/images/myImage.jpg";
        private const string CONTENT_TYPE = "image/jpeg";

        public MediaUpload()
        {
            InitializeComponent();
            if (_photoChooserTask == null)
            {
                _photoChooserTask = new PhotoChooserTask();
                _photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            }
            if (_cameraCaptureTask == null)
            {
                _cameraCaptureTask = new CameraCaptureTask();
                _cameraCaptureTask.Completed += cameraCaptureTask_Completed;
            }
        }

        private void Camera_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TakePhoto();
        }
        private void Media_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SelectPhoto();
        }

        private void SelectPhoto()
        {
            _photoChooserTask.Show();
        }
        private async void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                imageCrop.Source = image;
                await ProcessPhoto();
            }
        }

        private void TakePhoto()
        {
            _cameraCaptureTask.Show();
        }
        private async void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                e.ChosenPhoto.Position = 0;
                JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

                _width = info.Width;
                _height = info.Height;
                _orientation = info.Orientation;

                switch (info.Orientation)
                {
                    case ExifOrientation.TopLeft:
                    case ExifOrientation.Undefined:
                        _angle = 0;
                        break;
                    case ExifOrientation.TopRight:
                        _angle = 90;
                        break;
                    case ExifOrientation.BottomRight:
                        _angle = 180;
                        break;
                    case ExifOrientation.BottomLeft:
                        _angle = 270;
                        break;
                }

                if (_angle > 0d)
                {
                    _capturedImage = RotateStream(e.ChosenPhoto, _angle);
                }
                else
                {
                    _capturedImage = e.ChosenPhoto;
                }

                BitmapImage image = new BitmapImage();
                image.SetSource(_capturedImage);
                imageCrop.Source = image;

                await ProcessPhoto();
            }
        }

        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
        }

        private async Task ProcessPhoto()
        {
            try
            {
                gridAction.Visibility = System.Windows.Visibility.Collapsed;
                gridImageCrop.Visibility = System.Windows.Visibility.Visible;

                LayoutRoot.Height = imageCrop.Height;
                LayoutRoot.Width = imageCrop.Width;

                WriteBitmap(LayoutRoot);

                var image = new BitmapImage();
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                using (var local = new IsolatedStorageFileStream(IMAGE_PATH, FileMode.Open, store))
                {
                    image.SetSource(local);
                }

                _imageStream.Seek(0, SeekOrigin.Begin);

                gridImageCrop.Visibility = System.Windows.Visibility.Collapsed;
                gridUploading.Visibility = System.Windows.Visibility.Visible;

                string fileName = Context.User.Id + ".jpg";

                //**************** APPACITIVE HELP TEXT **************//
                //Following code demos two implementations of upload file
                // 1) Upload data by passing stream of bytes
                // 2) Upload a file from local storage

                //Upload file
                var uploadHandler = new Sdk.FileUpload(CONTENT_TYPE, fileName, 30);
                string uploadedFilename = string.Empty;

                //Case 1 : Using file byte data
                uploadedFilename = await uploadHandler.UploadAsync(_imageStream.ToArray());

                //Case 2 : Using file path
                //Uncomment it for testing
                //uploadedFilename = await uploadHandler.UploadFileAsync(IMAGE_PATH);

                Context.User["image"] = fileName;
                await Context.User.SaveAsync();
            }
            catch
            {
                MessageBox.Show("Ooops some error ocurred. Please try to upload profile picture. If it still failes contact support team.", "Something went wrong !!!", MessageBoxButton.OK);
            }

            gridImageCrop.Visibility = System.Windows.Visibility.Collapsed;
            gridAction.Visibility = System.Windows.Visibility.Visible;

            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void WriteBitmap(FrameworkElement element)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a new folder named DataFolder.
            if (!store.DirectoryExists("images")) store.CreateDirectory("images");

            WriteableBitmap wBitmap = new WriteableBitmap(element, null);
            _imageStream = new MemoryStream();
            wBitmap.SaveJpeg(_imageStream, (int)element.Width, (int)element.Height, 0, 100);
            using (var local = new IsolatedStorageFileStream(IMAGE_PATH, FileMode.Create, store))
            {
                local.Write(_imageStream.GetBuffer(), 0, _imageStream.GetBuffer().Length);
            }
        }
    }
}