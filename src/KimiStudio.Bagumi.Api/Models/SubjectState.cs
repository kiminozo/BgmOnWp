using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KimiStudio.Bagumi.Api.Models
{
    public class SubjectState
    {
        public string Comment { get; set; }
        [JsonProperty("ep_status")]
        public int EpisodeState { get; set; }
        public int LastTouch { get; set; }
        public SubjectStateInfo Status { get; set; }
        public string[] Tag { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
        
    }

    public class SubjectStateInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class SubjectStateUpdateInfo
    {
        public int SubjectId { get; set; }
        public string Comment { get; set; }
        public string[] Tags { get; set; }
        public int? Rating { get; set; }

        public string Method { get;  set; }

        public const string Wish = "wish";
        public const string Collect = "collect";
        public const string Do = "do";
        public const string OnHold = "on_hold";
        public const string Dropped = "dropped";
    }

    public static class TagBuilder
    {
        public static string GetTags(this string[] tags)
        {
            if (tags == null || tags.Length == 0) return null;

            var builder = new StringBuilder();
            foreach (var item in tags)
            {
                builder.Append(item);
                builder.Append(' ');
            }
            builder.Length--;
            return builder.ToString();
        }
    }

}