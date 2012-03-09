using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class EpisodeCollectionModel : IEnumerable<EpisodeModel>
    {
        private const int MaxLength = 52;

        public int SubjectId { get; set; }
        public IDictionary<int,int> EpisodeIds { get; private set; }
        public List<EpisodeModel> EpisodeModels { get; private set; }
        public bool IsUpdated { get; private set; }

        private EpisodeCollectionModel(){}

        public static EpisodeCollectionModel FromEpisodes(SubjectCommandResult result)
        {
            var collectionModel = new EpisodeCollectionModel();
            collectionModel.Update(result);
            return collectionModel;
        }

        public void Update(SubjectCommandResult result)
        {
            IsUpdated = false;
            var subject = result.Subject;
            SubjectId = subject.Id;
            if (subject.Eps != null)
            {
                EpisodeIds = subject.Eps.OrderBy(p => p.Sort).ToDictionary(k => k.Sort, v => v.Id);

                
                int length = subject.Eps.Count;

                IEnumerable<Episode> query;
                if (length > MaxLength) //长篇
                {
                    int state = result.SubjectState.EpisodeState;
                    state = state - 1 > 0 ? state - 1 : 0;
                    int skip = length - state < MaxLength ? length - MaxLength : state;
                    query = subject.Eps.Skip(skip).Take(MaxLength);
                }
                else
                {
                    query = subject.Eps;
                }
                var list = query.Select(EpisodeModel.FromEpisode).ToList();
                if (EpisodeModels == null || !list.SequenceEqual(EpisodeModels))
                {
                    EpisodeModels = list;
                    IsUpdated = true;
                }

                if (EpisodeModels != null && result.Progress != null)
                {
                    var progs = result.Progress.Episodes.ToDictionary(p => p.Id);
                    EpisodeModels.Apply(
                        model =>
                        {
                            EpisodeProgress prog;
                            if (!progs.TryGetValue(model.Id, out prog)) return;
                            model.Update((WatchState)prog.Status.Id);
                        });
                }

            }
        }

        public IEnumerable<int> GetBeforeIdList(EpisodeModel episodeModel)
        {
            if (EpisodeIds == null) return null;

            return EpisodeIds.Where(p => p.Key <= episodeModel.Sort).Select(p => p.Value).ToArray();
        }

        #region Implementation of IEnumerable

        public IEnumerator<EpisodeModel> GetEnumerator()
        {
            return EpisodeModels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        internal void UpdateEpisodeEnd(int sort)
        {
            if (EpisodeIds == null || EpisodeModels == null) return;
            EpisodeModels
                .Where(p => p.Sort <= sort)
                .Apply(m => m.Update(WatchState.Watched));
        }
    }

    public class EpisodeModel : PropertyChangedBase, IEquatable<EpisodeModel>
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
        public string AirDate { get; set; }

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
                                 Name = string.Format("ep.{0} {1}", episode.Sort, episode.Name),
                                 CnName = episode.NameCn,
                                 RemoteUrl = episode.Url,
                                 AirDate =  episode.AirDate,
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

        public bool Equals(EpisodeModel other)
        {
            return Id == other.Id;
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
