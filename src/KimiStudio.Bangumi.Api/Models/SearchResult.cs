using System.Collections.Generic;

namespace KimiStudio.Bangumi.Api.Models
{
    public class SearchResult
    {
        public int Results { get; set; }
        public IList<BagumiSubject> List { get; set; }
    }
}
