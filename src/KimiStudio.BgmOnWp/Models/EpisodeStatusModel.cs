using System.Collections.Generic;

namespace KimiStudio.BgmOnWp.Models
{
    public sealed class EpisodeStatusModel
    {
        public static EpisodeStatusModel Queue;
        public static EpisodeStatusModel Watched;
        public static EpisodeStatusModel WatchedEnd;
        public static EpisodeStatusModel Drop;
        public static EpisodeStatusModel Cancel;

        static EpisodeStatusModel()
        {
            Queue = new EpisodeStatusModel { Name = "�뿴" };
            Watched = new EpisodeStatusModel { Name = "����" };
            WatchedEnd = new EpisodeStatusModel { Name = "����" };
            Drop = new EpisodeStatusModel { Name = "����" };
            Cancel = new EpisodeStatusModel { Name = "����" };
        }

        private EpisodeStatusModel()
        {

        }

        public int Index { get; private set; }
        public string Name { get; private set; }
    }

    public sealed class EpisodeStatuses
    {
        public static IEnumerable<EpisodeStatusModel> Queue;
        public static IEnumerable<EpisodeStatusModel> Watched;
        public static IEnumerable<EpisodeStatusModel> Drop;
        public static IEnumerable<EpisodeStatusModel> None;

        static EpisodeStatuses()
        {
            Watched = new List<EpisodeStatusModel> { EpisodeStatusModel.Queue, EpisodeStatusModel.Drop, EpisodeStatusModel.Cancel };
            Queue = new List<EpisodeStatusModel>
                          {
                              EpisodeStatusModel.Watched,
                              EpisodeStatusModel.WatchedEnd,
                              EpisodeStatusModel.Drop,
                              EpisodeStatusModel.Cancel
                          };
            Drop = new List<EpisodeStatusModel>
                       {
                           EpisodeStatusModel.Watched,
                           EpisodeStatusModel.WatchedEnd,
                           EpisodeStatusModel.Queue,
                           EpisodeStatusModel.Cancel
                       };
            None = new List<EpisodeStatusModel>
                       {
                           EpisodeStatusModel.Watched,
                           EpisodeStatusModel.WatchedEnd,
                           EpisodeStatusModel.Queue,
                           EpisodeStatusModel.Drop
                       };
        }
    }
}