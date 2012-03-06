using System.Collections.Generic;
using Newtonsoft.Json;

namespace KimiStudio.Bangumi.Api.Models
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

    public class ProgressUpdateInfo
    {
        public const string Queue = "queue";
        public const string Watched = "watched";
        public const string Drop = "drop";
        public const string Remove = "remove";

        public int EpisodeId { get; set; }

        public IList<int> Episodes { get; set; }

        public string Method { get; set; }
    }

    
}
