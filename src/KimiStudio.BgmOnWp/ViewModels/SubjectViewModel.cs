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
        private readonly ILoadingService loadingService;

        public int Id { get; set; }
        public bool FromPin { get; set; }
        private SubjectStateModel subjectStateModel;

        #region Property
        private string name;
        private string cnName;
        private string doing;
        private bool isPined;
        private Uri imageSource;
        private string summary;
        private IEnumerable<CharacterModel> characters;
        private IEnumerable<StaffModel> staff;
        private EpisodeCollectionModel episodes;
        private IEnumerable<BlogModel> blogs;

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

        public Uri ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
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

        public EpisodeCollectionModel Episodes
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

        public bool IsPined
        {
            get { return isPined; }
            set
            {
                isPined = value;
                NotifyOfPropertyChange(() => IsPined);
            }
        }

        #endregion


        public SubjectViewModel(IProgressService progressService, IPromptManager promptManager, ILoadingService loadingService)
        {
            this.progressService = progressService;
            this.promptManager = promptManager;
            this.loadingService = loadingService;
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
            DisplayName = subject.Name;
            Name = subject.Name;
            CnName = subject.NameCn;
            ImageSource = subject.Images != null ? subject.Images.Common : null;
            Summary = subject.Summary;
            Doing = subject.Collection != null ? string.Format("{0}人在看", subject.Collection.Doing) : null;
            State = SubjectStateModel.FromSubjectState(Id, result.SubjectState);

            SetCharacters(subject);
            SetStaff(subject);
            SetEpisodes(result);
            SetBlogs(subject);

            IsPined = TileHelper.IsPined(this);

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
            TileHelper.PinTile(this);
        }
        #endregion

    }
}
