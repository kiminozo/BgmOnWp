using System;
using System.Globalization;
using System.Windows.Media;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class EpisodeModel
    {
        public int Id { get; set; }
        public int Sort { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string CnName { get; set; }
        public Uri RemoteUrl { get; set; }
        public WatchState WatchState { get; set; }
        public bool IsOnAir { get; set; }
        public Brush Fill { get; set; }

        private static readonly Brush Watched = new SolidColorBrush(Color.FromArgb(0xFF, 0x48, 0x97, 0xFF));
        private static readonly Brush UnWatched = new SolidColorBrush(Color.FromArgb(0xFF, 0xA1, 0xCF, 0xEF));
        private static readonly Brush UnAir = new SolidColorBrush(Colors.DarkGray);

        public static EpisodeModel FromEpisode(Episode episode)
        {
            return new EpisodeModel
                       {
                           Id = episode.Id,
                           Sort = episode.Sort,
                           Number = episode.Sort.ToString(CultureInfo.InvariantCulture),
                           Name = episode.Name,
                           CnName = episode.NameCn,
                           RemoteUrl = episode.Url,
                           IsOnAir = episode.Status == Episode.OnAir,
                           Fill = episode.Status == Episode.OnAir ? UnWatched : UnAir,
                       };
        }
    }

    public enum WatchState
    {
        None,
        /// <summary>
        /// 想看
        /// </summary>
        Queue,
        /// <summary>
        /// 看过
        /// </summary>
        Watched,
        /// <summary>
        /// 抛弃
        /// </summary>
        Drop
    }
}
