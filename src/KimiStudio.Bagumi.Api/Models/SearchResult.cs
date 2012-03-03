using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KimiStudio.Bagumi.Api.Models
{
    public class SearchResult
    {
        public int Results { get; set; }
        public IList<BagumiSubject> List { get; set; }
    }
}
