using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class FavoriteViewModel : Screen, IPrompt
    {
        #region Property
        private int rating;
        private string comment;
        private string tags;
        private int index;



        public int Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                NotifyOfPropertyChange(() => Rating);
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                NotifyOfPropertyChange(() => Comment);
            }
        }

        public string Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                NotifyOfPropertyChange(() => Tags);
            }
        }

        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }
        #endregion

        private readonly static IList<string> ActionTypes;
        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;
        private SubjectStateModel subjectState;

        static FavoriteViewModel()
        {
            ActionTypes = new List<string>
                        {
                            SubjectStateUpdateInfo.Wish,
                            SubjectStateUpdateInfo.Collect,
                            SubjectStateUpdateInfo.Do,
                            SubjectStateUpdateInfo.OnHold,
                            SubjectStateUpdateInfo.Dropped
                        };
        }

        public FavoriteViewModel(IProgressService progressService, IPromptManager promptManager)
        {
            this.progressService = progressService;
            this.promptManager = promptManager;
            DisplayName = "收藏";
        }

        public void SetUp(SubjectStateModel state)
        {
            subjectState = state;
            Tags = state.Tag.GetTags();
            Comment = state.Comment;
            Rating = state.Rating;
            Index = ActionTypes.IndexOf(state.Type);
        }

        #region Implementation of IPrompt

        public void PromptResult(bool canceled)
        {
            if (canceled) return;

            progressService.Show("提交中\u2026");
            var command = new SubjectStateUpdateCommand(new SubjectStateUpdateInfo
                                                            {
                                                                Comment = Comment,
                                                                Method = ActionTypes[Index],
                                                                Rating = Rating,
                                                                SubjectId = subjectState.SubjectId,
                                                                Tags = SipTags()
                                                            }, AuthStorage.Auth);
            command.BeginExecute(UpdateCallBack, command);
        }



        #endregion

        private string[] SipTags()
        {
            if (string.IsNullOrEmpty(Tags)) return null;

            return tags.Split(',', ' ');
        }

        private void UpdateCallBack(IAsyncResult asyncResult)
        {
            try
            {
                var command = (SubjectStateUpdateCommand)asyncResult.AsyncState;
                var result = command.EndExecute(asyncResult);
                if(result.LastTouch != 0)
                {
                    subjectState.Update(result);
                }
                progressService.Hide();
                promptManager.ToastInfo("收藏成功");
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                progressService.Hide();
                promptManager.ToastError(err, "收藏失败");
            }
            finally
            {
                
            }
        }
    }

}
