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
        private readonly ILoadingService loadingService;
        private readonly INavigationService navigation;

        public int Id { get; set; }
        public bool FromPin { get; set; }
       

        #region Property

        private Uri imageSource;
        private SubjectModel subjectModel;
        private SubjectStateModel subjectStateModel;
        private IEnumerable<CharacterModel> characters;
        private IEnumerable<StaffModel> staff;
        private EpisodeCollectionModel episodes;
        private IEnumerable<BlogModel> blogs;

        public Uri ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        public SubjectModel Subject
        {
            get { return subjectModel; }
            set
            {
                subjectModel = value;
                NotifyOfPropertyChange(() => Subject);
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

        public EpisodeCollectionModel Episodes
        {
            get { return episodes; }
            set
            {
                episodes = value;
                NotifyOfPropertyChange(() => Episodes);
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


        public SubjectViewModel(IProgressService progressService, IPromptManager promptManager, ILoadingService loadingService, INavigationService navigation)
        {
            this.progressService = progressService;
            this.promptManager = promptManager;
            this.loadingService = loadingService;
            this.navigation = navigation;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            if (FromPin)
            {
                Login();
            }
            else
            {
                LoadSubject();
            }
        }

        private void Login()
        {
            loadingService.Show("登录中\u2026");
            var task = CommandTaskFactory.Create(new LoginCommand(AuthStorage.UserName, AuthStorage.Password));
            task.Result(auth =>
            {
                loadingService.Hide();
                AuthStorage.Auth = auth;
                LoadSubject();
            });
            task.Exception(err =>
            {
                loadingService.Hide();
                promptManager.ToastError(err);
            });
            task.Start();
        }

        private void LoadSubject()
        {
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

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            progressService.Hide();
        }

        #region SetSubject
        private void SetSubject(SubjectCommandResult result)
        {
            var subject = result.Subject;
            if (subject == null) return;

            Subject = SubjectModel.FromSubject(subject);
            DisplayName = Subject.Name;
            ImageSource = Subject.ImageSource;

            State = SubjectStateModel.FromSubjectState(Id, result.SubjectState);

            SetCharacters(subject);
            SetStaff(subject);
            SetEpisodes(result);
            SetBlogs(subject);
        }

        private void SetEpisodes(SubjectCommandResult result)
        {
            if (Episodes == null)
            {
                Episodes = EpisodeCollectionModel.FromEpisodes(result);
            }
            else
            {
                Episodes.Update(result);
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
            if (subject.Blog != null)
            {
                Blogs = subject.Blog.Select(BlogModel.FromBlog);
            }
        }

        #endregion

        #region Public
        public void Favorite()
        {
            if (State == null) return;

            promptManager.PopupFor<FavoriteViewModel>()
                .Setup(model => model.SetUp(State))
                .SetTitleBackground("BangumiBlue")
                .SetEnableCancel()
                .Show();
        }

        public void TapEpisodeItem(EpisodeModel episode)
        {
            var canEdit = episode.IsOnAir && subjectStateModel.IsWatching;
            promptManager.PopupFor<EpisodeStatusViewModel>()
                .Setup(x => x.Setup(episode, Episodes, canEdit))
                .SetTitleBackground("BangumiPink")
                .SetEnableCancel(canEdit)
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

        public void TapBlogItem(BlogModel blogModel)
        {
            if (blogModel.RemoteUrl == null) return;

            var task = new WebBrowserTask { Uri = blogModel.RemoteUrl };
            task.Show();
        }

        public void Pin()
        {
            if(Subject == null)return;

            if (Subject.IsPined())
            {
                promptManager.ToastWarn("已经固定到桌面过了");
                return;
            }
            Subject.PinTile();
        }

        public void Home()
        {
            navigation.UriFor<MainPageViewModel>()
                .WithParam(x => x.Authed,true)
                .Navigate();
        }
        #endregion

    }
}
