using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KimiStudio.Bagumi.Api.Models
{
    public class DefaultResult
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public string Request { get; set; }

        public bool IsSuccess()
        {
            return Code == 200;
        }
    }
}
