﻿using System;
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
        private IEnumerable<BlogModel> blogs;
        private IList<int> episodeIds = new List<int>(0);

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

        public IEnumerable<BlogModel> Blogs
        {
            get { return blogs; }
            set
            {
                blogs = value;
                NotifyOfPropertyChange(() => Blogs);
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

        #region SetSubject
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

            SetCharacters(subject);
            SetStaff(subject);
            SetEpisodes(result);
            SetBlogs(subject);

        }

        private void SetEpisodes(SubjectCommandResult result)
        {
            var subject = result.Subject;
            if (subject.Eps != null)
            {
                episodeIds = subject.Eps.OrderBy(p => p.Sort).Select(p => p.Id).ToList();

                const int maxLength = 52;
                int length = subject.Eps.Count;

                IEnumerable<Episode> query;
                if (length > maxLength) //长篇
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
                if(Episodes == null || !list.SequenceEqual(Episodes))
                {
                    Episodes = list;
                }

                if (Episodes != null && result.Progress != null)
                {
                    var progs = result.Progress.Episodes.ToDictionary(p => p.Id);
                    Episodes.Apply(model =>
                                       {
                                           EpisodeProgress prog;
                                           if (!progs.TryGetValue(model.Id, out prog)) return;
                                           model.Update((WatchState) prog.Status.Id);
                                       });
                }
            }
        }

        private void SetStaff(Subject subject)
        {
            if (subject.Staff != null)
            {
                Staff = subject.Staff.Select(StaffModel.FromStaffItem);
            }
        }

        private void SetCharacters(Subject subject)
        {
            if (subject.Characters != null)
            {
                Characters = subject.Characters.OrderBy(p => p.Id).Select(CharacterModel.FromCharacter);
            }
        }

        private void SetBlogs(Subject subject)
        {
            if(subject.Blog != null)
            {
                Blogs = subject.Blog.Select(BlogModel.FromBlog);
            }
        }

        #endregion

        #region Public
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
                .Setup(x => x.Setup(episode, episodeIds))
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
        #endregion

    }
}
