using System;
using System.Collections.Generic;
using System.Diagnostics;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class CharacterViewModel
    {
        public Uri CharacterImage { get; set; }
        public string CharacterName { get; set; }
        public string CvName { get; set; }

        public static CharacterViewModel FromCharacter(Character character)
        {
            try
            {
                return new CharacterViewModel
                           {
                               CharacterImage = character.Images != null ? character.Images.Grid : null,//TODO:defultImage
                               CharacterName = character.Name,
                               CvName = ToCvName(character.Actors)
                           };
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return new CharacterViewModel();
            }
        }

        private static string ToCvName(IList<Actor> actors)
        {
            if (actors == null || actors.Count < 1) return null;

            return "CV: " + actors[0].Name;
        }
    }
}