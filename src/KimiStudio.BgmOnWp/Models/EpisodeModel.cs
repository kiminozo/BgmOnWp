using System;
using System.Globalization;
using System.Windows.Media;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class EpisodeModel : PropertyChangedBase
    {
        private Color fill;

        public int Id { get; set; }
        public int Sort { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string CnName { get; set; }
        public Uri RemoteUrl { get; set; }
        public WatchState WatchState { get; set; }
        public bool IsOnAir { get; set; }

        public Color Fill
        {
            get { return fill; }
            set
            {
                fill = value;
                NotifyOfPropertyChange(() => Fill);
            }
        }



        public static EpisodeModel FromEpisode(Episode episode)
        {
            try
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
                                 Fill = episode.Status == Episode.OnAir ? WatchStateColors.UnWatched : WatchStateColors.UnAir
                             };
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Update(WatchState state)
        {
            if (WatchState == state) return;

            switch (state)
            {
                case WatchState.Queue:
                    Fill = WatchStateColors.Queue;
                    break;
                case WatchState.Watched:
                    Fill = WatchStateColors.Watched;
                    break;
                case WatchState.Drop:
                    Fill = WatchStateColors.Drop;
                    break;
                default:
                    Fill = IsOnAir ? WatchStateColors.UnWatched : WatchStateColors.UnAir;
                    break;

            }
            WatchState = state;
        }
    }

    public class WatchStateColors
    {
        public static readonly Color Queue;
        public static readonly Color Watched;
        public static readonly Color UnWatched;
        public static readonly Color UnAir;
        public static readonly Color Drop;

        static WatchStateColors()
        {
            Queue = Color.FromArgb(0xFF, 0xFF, 0xAD, 0xD1);
            Watched = Color.FromArgb(0xFF, 0x48, 0x97, 0xFF);
            UnWatched = Color.FromArgb(0xFF, 0xA1, 0xCF, 0xEF);
            UnAir = Colors.DarkGray;
            Drop = Colors.Gray;
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
