using System;
using System.Linq;
using System.Collections.Generic;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Api;
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
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
            var command = new GetSubjectCommand(Id);
            command.Execute(Handle);
        }

        private void Handle(SubjectMessage message)
        {
            progressService.Hide();

            if (message.Cancelled) return;
            SetSubject(message.Subject);
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

    public class CharacterViewModel
    {
        public Uri CharacterImage { get; set; }
        public string CharacterName  { get; set; }
        public string CvName { get; set; }

        public static CharacterViewModel FromCharacter(Character character)
        {
            return new CharacterViewModel
                       {
                           CharacterImage = character.Images.Grid,
                           CharacterName = character.Name,
                           CvName = ToCvName(character.Actors)
                       };
        }

        private static string ToCvName(IList<Actor> actors)
        {
            if (actors == null || actors.Count < 1) return null;

            return "CV: " + actors[0].Name;
        }
    }
}
