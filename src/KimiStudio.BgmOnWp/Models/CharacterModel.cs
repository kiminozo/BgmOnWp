using System;
using System.Collections.Generic;
using System.Diagnostics;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class CharacterModel
    {
        public Uri CharacterImage { get; set; }
        public string CharacterName { get; set; }
        public string CvName { get; set; }
        public Uri RemoteUrl { get; set; }


        public static CharacterModel FromCharacter(Character character)
        {
            try
            {
                return new CharacterModel
                           {
                               CharacterImage = character.Images != null ? character.Images.Grid : null,//TODO:defultImage
                               CharacterName = character.Name,
                               CvName = ToCvName(character.Actors),
                               RemoteUrl = character.Url
                           };
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return new CharacterModel();
            }
        }

        private static string ToCvName(IList<Actor> actors)
        {
            if (actors == null || actors.Count < 1) return null;

            return "CV: " + actors[0].Name;
        }
    }
}