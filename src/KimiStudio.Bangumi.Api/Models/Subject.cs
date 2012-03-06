using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KimiStudio.Bangumi.Api.Models
{
    public class Subject
    {
        [JsonProperty("air_date")]
        public string AirDate { get; set; }

        [JsonProperty("air_weekday")]
        public int AirWeekday { get; set; }
        public IList<Blog> Blog { get; set; }
        public BagumiSubjectTap Collection { get; set; }

        [JsonProperty("crt")]
        public IList<Character> Characters { get; set; }
        public IList<Episode> Eps { get; set; }
        public int Id { get; set; }
        public ImageData Images { get; set; }
        public string Name { get; set; }
        [JsonProperty("name_cn")]
        public string NameCn { get; set; }
        public IList<StaffItem> Staff { get; set; }
        public string Summary { get; set; }
        public IList<Topic> Topic { get; set; }
        public int Type { get; set; }
        public Uri Url { get; set; }
    }

    public class Topic
    {
        public int Id { get; set; }
        public int Lastpost { get; set; }

        [JsonProperty("main_id")]
        public int MainId { get; set; }
        public int Replies { get; set; }
        public int Timetamp { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public User User { get; set; }
    }

    public class StaffItem
    {
        public int Collects { get; set; }
        public int Comment { get; set; }
        public int Id { get; set; }
        public ImageData Images { get; set; }
        public CharacterInfo Info { get; set; }
        public IList<string> Jobs { get; set; }
        public string Name { get; set; }

        [JsonProperty("name_cn")]
        public string NameCn { get; set; }
        public string RoleName { get; set; }
        public Uri Url { get; set; }
    }

    public class Episode
    {
        public const string OnAir = "Air";

        public string AirDate { get; set; }
        public int Comment { get; set; }
        public string Desc { get; set; }
        public string Duration { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("name_cn")]
        public string NameCn { get; set; }
        public int Sort { get; set; }
        public string Status { get; set; }//Air,NA
        public int Type { get; set; }
        public Uri Url { get; set; }
    }

    public class Blog
    {
        public DateTime Dateline { get; set; }
        public int Id { get; set; }
        public int Replies { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Timetamp { get; set; }
        public Uri Url { get; set; }
        public User User { get; set; }
    }

    public class Character
    {
        public IList<Actor> Actors { get; set; }
        public int Collects { get; set; }
        public int Id { get; set; }
        public ImageData Images { get; set; }
        public CharacterInfo Info { get; set; }

        public string Name { get; set; }
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
        public Uri Url { get; set; }
    }

    public class CharacterInfo
    {
        //public Alias Alias { get; set; }
        public string Brith { get; set; }
        //public string Bloodtype { get; set; }
        //public string Bwh { get; set; }
        public string Gender { get; set; }
        // public string Height { get; set; }
        [JsonProperty("name_cn")]
        public string NameCn { get; set; }
        // public string Source { get; set; }
        // public string Weight { get; set; }
        
    }

    public class Alias
    {
        public string Jp { get; set; }
        public string Romaji { get; set; }
        public string Zh { get; set; }
    }

    public class Actor
    {
        public int Id { get; set; }
        public ImageData Image { get; set; }
        public string Name { get; set; }
    }

    public class ImageData
    {
        public Uri Common { get; set; }
        public Uri Grid { get; set; }
        public Uri Large { get; set; }
        public Uri Medium { get; set; }
        public Uri Small { get; set; }
    }

    public class User
    {
        public Avatar Avatar { get; set; }
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Sign { get; set; }
        public Uri Url { get; set; }
        public string UserName { get; set; }
    }

    public class AuthUser : User
    {
        [JsonProperty]
        public string Auth { get; set; }

        [JsonProperty("auth_encode")]
        public string AuthEncode { get; set; }

    }

    public class Avatar
    {
        public Uri Large { get; set; }
        public Uri Medium { get; set; }
        public Uri Small { get; set; }
    }

    public class BagumiSubjectTap
    {
        public int Collect { get; set; }
        public int Doing { get; set; }
        public int Dropped { get; set; }

        [JsonProperty("on_hold")]
        public int OnHold { get; set; }
        public int Wish { get; set; }
    }
}
