using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Appacitive.Mobile.Windows.Todo.Utility;

namespace Appacitive.Mobile.WindowsPhone7.Todo.Model
{
    public class ImageDataContext : INotifyPropertyChanged
    {
        private string _image;
        public string Image
        {
            get
            {
                return string.IsNullOrWhiteSpace(_image) ? Constants.DEFAULT_PHOTO_ADD : _image;
            }
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }
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
