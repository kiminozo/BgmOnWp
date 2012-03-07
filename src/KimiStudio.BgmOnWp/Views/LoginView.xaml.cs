using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace KimiStudio.BgmOnWp.Views
{
    public partial class LoginView : PhoneApplicationPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

      

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            BindingExpression bindingExpr = username.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpr == null) return;
            bindingExpr.UpdateSource();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpr = password.GetBindingExpression(PasswordBox.PasswordProperty);
            if (bindingExpr == null) return;
            bindingExpr.UpdateSource();
        }
    }
}