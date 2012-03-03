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
    }

    public class SubjectStateInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}