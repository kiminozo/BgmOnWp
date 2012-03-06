using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectStateModel : PropertyChangedBase
    {
        private static readonly string[] StateNames = new[]
        {
           "加入我的收藏","我想看这部","我看过了这部","我正在看这部","我搁置了这部","我已抛弃这部"
        };

        public int SubjectId { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public string[] Tag { get; set; }
        public int Rating { get; set; }

        public bool IsWatching
        {
            get { return SubjectStateUpdateInfo.Do == Type; }
        }

        private bool isFavorited;
        public bool IsFavorited
        {
            get { return isFavorited; }
            set
            {
                isFavorited = value;
                NotifyOfPropertyChange(() => IsFavorited);
            }
        }

        private string stateName;

        public string StateName
        {
            get { return stateName; }
            set
            {
                stateName = value;
                NotifyOfPropertyChange(() => StateName);
            }
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
            if (state.Status == null)
            {
                Type = null;
                StateName = StateNames[0];
            }
            else
            {
                Type = state.Status.Type;
                StateName = StateNames[state.Status.Id];
            }
            Type = state.Status == null ? null : state.Status.Type;
            IsFavorited = state.IsFavorited();
        }
    }
}
