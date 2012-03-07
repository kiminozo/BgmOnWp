using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KimiStudio.Bangumi.Api.Models
{
    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {

        }

        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
