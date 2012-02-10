using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KimiStudio_BgmOnWp
{
    public class ItemViewModel : INotifyPropertyChanged 
    {
        private string _lineOne;
        /// <summary>
        /// 示例 ViewModel 属性; 此属性在视图中使用，以使用 Binding 来显示其值。
        /// </summary>
        /// <returns></returns>
        public string LineOne 
        {
            get 
            {
                return _lineOne;
            }
            set 
            {
                _lineOne = value;
                NotifyPropertyChanged("LineOne");
            }
        }
        
        private string _lineTwo;
        /// <summary>
        /// 示例 ViewModel 属性; 此属性在视图中使用，以使用 Binding 来显示其值。
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                _lineTwo = value;
                NotifyPropertyChanged("LineTwo");
            }
        }

        private string _lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                _lineThree = value;
                NotifyPropertyChanged("LineThree");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) 
        {
            if (null != PropertyChanged) 
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}