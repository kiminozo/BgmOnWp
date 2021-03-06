﻿using System;
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
using Microsoft.Phone.Controls;

namespace KimiStudio.BgmOnWp.Views
{
    public partial class SubjectView 
    {
        public SubjectView()
        {
            InitializeComponent();
        }

        private void ApplicationBar_StateChanged(object sender, Microsoft.Phone.Shell.ApplicationBarStateChangedEventArgs e)
        {
            ApplicationBar.BackgroundColor = e.IsMenuVisible
                                                 ? (Color) Application.Current.Resources["PhoneBackgroundColor"]
                                                 : Colors.Transparent;
        }
    }
}