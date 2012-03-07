using System;
using System.Collections.Generic;
using System.Diagnostics;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class CharacterModel
    {
        public Uri CharacterImage { get; set; }
        public string CharacterName { get; set; }
        public string CnName { get; set; }
        public string Role { get; set; }
        public string CvName { get; set; }
        public Uri RemoteUrl { get; set; }


        public static CharacterModel FromCharacter(Character character)
        {
            try
            {
                return new CharacterModel
                           {
                               CharacterImage = CharacterImageUri(character.Images),
                               CharacterName = character.Name,
                               CvName = ToCvName(character.Actors),
                               Role = character.RoleName,
                               CnName = character.Info == null ? null : character.Info.NameCn,
                               RemoteUrl = character.Url
                           };
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                return new CharacterModel();
            }
        }

        private static Uri CharacterImageUri(ImageData image)
        {
            return image != null ? image.Grid : new Uri("/Images/info_only_m.png", UriKind.Relative);
        }

        private static string ToCvName(IList<Actor> actors)
        {
            if (actors == null || actors.Count < 1) return null;

            return "CV: " + actors[0].Name;
        }
    }
}