﻿#pragma checksum "C:\Users\apalsapure\SkyDrive\Windows Phone Apps\Appacitive.Mobile.WindowsPhone7.Todo\Appacitive.Mobile.WindowsPhone7.Todo\DetailsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "767417097690C756D07E381233789AF3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Appacitive.Mobile.WindowsPhone7.Todo {
    
    
    public partial class DetailsPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar progress;
        
        internal System.Windows.Controls.Grid gTodoList;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox MainLongListSelector;
        
        internal System.Windows.Controls.Grid gAddList;
        
        internal System.Windows.Controls.TextBox txtListItemName;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Appacitive.Mobile.WindowsPhone7.Todo;component/DetailsPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.progress = ((System.Windows.Controls.ProgressBar)(this.FindName("progress")));
            this.gTodoList = ((System.Windows.Controls.Grid)(this.FindName("gTodoList")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.MainLongListSelector = ((System.Windows.Controls.ListBox)(this.FindName("MainLongListSelector")));
            this.gAddList = ((System.Windows.Controls.Grid)(this.FindName("gAddList")));
            this.txtListItemName = ((System.Windows.Controls.TextBox)(this.FindName("txtListItemName")));
        }
    }
}

