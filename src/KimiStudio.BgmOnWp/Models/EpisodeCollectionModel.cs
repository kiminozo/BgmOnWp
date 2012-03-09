using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

      
        #region Implementation of IEnumerable

        private static readonly IEnumerator<EpisodeModel> EmptyEnumerator = Enumerable.Empty<EpisodeModel>().GetEnumerator();
        public IEnumerator<EpisodeModel> GetEnumerator()
        {
            if (EpisodeModels == null) return EmptyEnumerator;
            return EpisodeModels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public IEnumerable<int> GetBeforeIdList(EpisodeModel episodeModel)
        {
            if (EpisodeIds == null) return null;

            return EpisodeIds.Where(p => p.Key <= episodeModel.Sort).Select(p => p.Value).ToArray();
        }


        internal void UpdateEpisodeEnd(int sort)
        {
            if (EpisodeIds == null || EpisodeModels == null) return;
            EpisodeModels
                .Where(p => p.Sort <= sort && (p.WatchState != WatchState.Drop || p.WatchState != WatchState.Watched))
                .Apply(m => m.Update(WatchState.Watched));
        }
    }
}