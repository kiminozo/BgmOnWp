using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace KimiStudio.Bagumi.Api.Models
{
    public class Progress
    {
        [JsonProperty("eps")]
        public IList<EpisodeProgress> Episodes { get; set; }

        [JsonProperty("subject_id")]
        public int SubjectId { get; set; }
    }

    public class EpisodeProgress
    {
        public int Id { get; set; }
        public ProgressState Status { get; set; }
    }

    public class ProgressState
    {
        public int Id { get; set; }

        [JsonProperty("cn_name")]
        public string CssName { get; set; }

        [JsonProperty("css_name")]
        public string CnName { get; set; }

        [JsonProperty("url_name")]
        public string UrlName { get; set; }
    }

    
}
