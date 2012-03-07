using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;
using Microsoft.Phone.Tasks;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectViewModel : Screen
    {
        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;

        public int Id { get; set; }
        private SubjectStateModel subjectStateModel;

        #region Property
        private string name;
        private string cnName;
        private string doing;
        private bool favorited;
        private Uri uriSource;
        private string summary;
        private IEnumerable<CharacterModel> characters;
        private IEnumerable<StaffModel> staff;
        private IEnumerable<EpisodeModel> episodes;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string CnName
        {
            get { return cnName; }
            set
            {
                cnName = value;
                NotifyOfPropertyChange(() => CnName);
            }
        }

        public Uri UriSource
        {
            get { return uriSource; }
            set
            {
                uriSource = value;
                NotifyOfPropertyChange(() => UriSource);
            }
        }

        public string Summary
        {
            get { return summary; }
            set
            {
                summary = value;
                NotifyOfPropertyChange(() => Summary);
            }
        }

        public IEnumerable<CharacterModel> Characters
        {
            get { return characters; }
            set
            {
                characters = value;
                NotifyOfPropertyChange(() => Characters);
            }
        }

        public IEnumerable<StaffModel> Staff
        {
            get { return staff; }
            set
            {
                staff = value;
                NotifyOfPropertyChange(() => Staff);
            }
        }

        public IEnumerable<EpisodeModel> Episodes
        {
            get { return episodes; }
            set
            {
                episodes = value;
                NotifyOfPropertyChange(() => Episodes);
            }
        }

        public SubjectStateModel State
        {
            get { return subjectStateModel; }
            set
            {
                subjectStateModel = value;
                NotifyOfPropertyChange(() => State);
            }
        }

        public string Doing
        {
            get { return doing; }
            set
            {
                doing = value;
                NotifyOfPropertyChange(() => Doing);
            }
        }

        #endregion


        public SubjectViewModel(IProgressService progressService, IPromptManager promptManager)
        {
            this.progressService = progressService;
            this.promptManager = promptManager;

        }

        protected override void OnActivate()
        {
            base.OnActivate();
            progressService.Show("加载中\u2026");
            var task = CommandTaskFactory.Create(new SubjectCommand(Id, AuthStorage.Auth));
            task.Result(result =>
                            {
                                progressService.Hide();
                                SetSubject(result);
                            });
            task.Exception(err =>
                               {
                                   progressService.Hide();
                                   promptManager.ToastError(err, "错误");
                               });
            task.Start();
        }

        private void SetSubject(SubjectCommandResult result)
        {
            var subject = result.Subject;
            DisplayName = subject.Name;
            Name = subject.Name;
            CnName = subject.NameCn;
            UriSource = subject.Images.Large;
            Summary = subject.Summary;
            Doing = string.Format("{0}人在看", subject.Collection.Doing);
            State = SubjectStateModel.FromSubjectState(Id, result.SubjectState);

            if (subject.Characters != null)
            {
                Characters = subject.Characters.OrderBy(p => p.Id).Select(CharacterModel.FromCharacter);
            }
            if (subject.Staff != null)
            {
                Staff = subject.Staff.Select(StaffModel.FromStaffItem);
            }
            if (subject.Eps != null)
            {
                const int maxLength = 52;
                int length = subject.Eps.Count;

                IEnumerable<Episode> query;
                if (length > maxLength)//长篇
                {
                    int state = result.SubjectState.EpisodeState;
                    state = state - 1 > 0 ? state - 1 : 0;
                    int skip = length - state < maxLength ? length - maxLength : state;
                    query = subject.Eps.Skip(skip).Take(maxLength);
                }
                else
                {
                    query = subject.Eps;
                }
                var list = query.Select(EpisodeModel.FromEpisode).ToList();
                if (result.Progress != null)
                {
                    var progs = result.Progress.Episodes.ToDictionary(p => p.Id);
                    list.Apply(model =>
                                   {
                                       EpisodeProgress prog;
                                       if (!progs.TryGetValue(model.Id, out prog)) return;
                                       model.Update((WatchState)prog.Status.Id);

                                   });
                }
                Episodes = list;
            }
        }

        public void Favorite()
        {
            promptManager.PopupFor<FavoriteViewModel>()
                .Setup(model => model.SetUp(State))
                .SetTitleBackground("BangumiBlue")
                .EnableCancel
                .Show();
        }

        public void TapEpisodeItem(EpisodeModel episode)
        {
            if (!episode.IsOnAir || !subjectStateModel.IsWatching) return;
            promptManager.PopupFor<EpisodeStatusViewModel>()
                .Setup(x => x.Setup(episode))
                .SetTitleBackground("BangumiPink")
                .EnableCancel
                .Show();
        }

        public void TapCharacterItem(CharacterModel character)
        {
            if (character.RemoteUrl == null) return;

            var task = new WebBrowserTask { Uri = character.RemoteUrl };
            task.Show();
        }

        public void TapStaffItem(StaffModel staffModel)
        {
            if (staffModel.RemoteUrl == null) return;

            var task = new WebBrowserTask { Uri = staffModel.RemoteUrl };
            task.Show();
        }

    }
}
