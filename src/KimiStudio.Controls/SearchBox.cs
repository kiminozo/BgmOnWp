using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KimiStudio.Controls
{
    public class SearchBox : Control
    {
        public static readonly DependencyProperty KeywordProperty =
            DependencyProperty.Register("Keyword", typeof(string), typeof(SearchBox),
                                        new PropertyMetadata(default(string)));

        public string Keyword
        {
            get { return (string) GetValue(KeywordProperty); }
            set { SetValue(KeywordProperty, value); }
        }

        private TextBox textBox;

        public event EventHandler SearchKeyDown;

      

        public SearchBox()
        {
            DefaultStyleKey = typeof (SearchBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textBox = (TextBox) GetTemplateChild("textBox");
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //BindingExpression bindingExpr = GetBindingExpression(KeywordProperty);
            //if (bindingExpr == null) return;
            //bindingExpr.UpdateSource();
            Keyword = textBox.Text;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               // e.Handled = true;
                Focus();
                OnSearchKeyDown();
            }
        }

        public void OnSearchKeyDown()
        {
            var handler = SearchKeyDown;
            if (handler == null) return;
            handler(this, EventArgs.Empty);
        }
    }
}
