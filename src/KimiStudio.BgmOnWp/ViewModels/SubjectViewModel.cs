﻿using System;
using System.Linq;
using System.Collections.Generic;
using Caliburn.Micro;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectViewModel : Screen
    {
        private readonly IProgressService progressService;

        public int Id { get; set; }

        #region Property
        private string name;
        private string cnName;
        private Uri uriSource;
        private string summary;
        private IEnumerable<CharacterViewModel> characters;

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

        public IEnumerable<CharacterViewModel> Characters
        {
            get { return characters; }
            set
            {
                characters = value;
                NotifyOfPropertyChange(() => Characters);
            }
        }


        #endregion


        public SubjectViewModel(IProgressService progressService)
        {
            this.progressService = progressService;
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
                Characters = subject.Characters.Select(CharacterViewModel.FromCharacter);
            }
        }
    }
}
