﻿using System;
using System.Linq;
using System.Collections.Generic;
using Caliburn.Micro;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;
using Microsoft.Phone.Tasks;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectViewModel : Screen
    {
        private readonly IProgressService progressService;
        private readonly INavigationService navigationService;
        private readonly IPromptManager promptManager;

        public int Id { get; set; }

        #region Property
        private string name;
        private string cnName;
        private Uri uriSource;
        private string summary;
        private IEnumerable<CharacterModel> characters;
        private IEnumerable<StaffModel> staff;

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


        #endregion


        public SubjectViewModel(IProgressService progressService, INavigationService navigationService, IPromptManager promptManager)
        {
            this.progressService = progressService;
            this.navigationService = navigationService;
            this.promptManager = promptManager;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            progressService.Show("加载中\u2026");
            var command = new GetSubjectCommand(Id, AuthStorage.Auth);
            command.BeginExecute(CallBack, command);
        }

        private void CallBack(IAsyncResult asyncResult)
        {
            try
            {
                var command = (GetSubjectCommand)asyncResult.AsyncState;
                var result = command.EndExecute(asyncResult);
                SetSubject(result);
            }
            catch (Exception)
            {
                //TODO:
            }
            finally
            {
                progressService.Hide();
            }
        }

        private void SetSubject(Subject subject)
        {
            DisplayName = subject.Name;
            Name = subject.Name;
            CnName = subject.NameCn;
            UriSource = subject.Images.Large;
            Summary = subject.Summary;

            if (subject.Characters != null)
            {
                Characters = subject.Characters.Select(CharacterModel.FromCharacter);
            }
            if(subject.Staff != null)
            {
                Staff = subject.Staff.Select(StaffModel.FromStaffItem);
            }
        }

        public void Favorite()
        {
            //            windowManager.ShowPopup(new FavoriteViewModel());
            //navigationService.UriFor<FavoriteViewModel>().Navigate();
            promptManager.ShowPopup(IoC.Get<FavoriteViewModel>());
        }

        public void OnTapCharacterItem(CharacterModel character)
        {
            if (character.RemoteUrl == null) return;

            var task = new WebBrowserTask { Uri = character.RemoteUrl };
            task.Show();
        }

        public void OnTapStaffItem(StaffModel staffModel)
        {
            if (staffModel.RemoteUrl == null) return;

            var task = new WebBrowserTask { Uri = staffModel.RemoteUrl };
            task.Show();
        }
    }
}
