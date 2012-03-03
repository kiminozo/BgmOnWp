using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectStateModel
    {
        public int SubjectId { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public string[] Tag { get; set; }
        public int Rating { get; set; }

        public bool IsWatching
        {
            get { return SubjectStateUpdateInfo.Do == Type; }
        }

        public static SubjectStateModel FromSubjectState(int id, SubjectState state)
        {
            var model = new SubjectStateModel
                       {
                           SubjectId = id,
                       };
            model.Update(state);
            return model;
        }

        public void Update(SubjectState state)
        {
            Tag = state.Tag;
            Comment = state.Comment;
            Rating = state.Rating;
            Type = state.Status == null ? null : state.Status.Type;
        }
    }
}
