using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DataStoreService.Services
{
    public class FileStorageService : IStorageService
    {
        private const string RootPath = "/App_Data/Profiles";

        /// <summary>
        /// Creates a new profile storage.
        /// </summary>
        /// <param name="key">Public key value</param>
        /// <param name="data">Data blob</param>
        /// <returns></returns>
        public string CreateProfile(Profile profile)
        {
            var profileId = Guid.NewGuid().ToString().Replace("-", "");

            var profilePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{RootPath}/{profileId}";

            // Make sure there's a directory
            var dirPath = Path.GetDirectoryName(profilePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var profileJson = JsonConvert.SerializeObject(profile);

            File.WriteAllText(profilePath, profileJson);

            return profileId;
        }

        /// <summary>
        /// Gets the profile for a given identifier.
        /// </summary>
        /// <param name="id">Profile identifier</param>
        /// <returns>Data in a string profile</returns>
        public string GetProfile(string id)
        {
            var profilePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{RootPath}/{id}";

            if (!File.Exists(profilePath))
            {
                throw new FileNotFoundException();
            }

            var profileData = File.ReadAllText(profilePath);
            var profile = Newtonsoft.Json.JsonConvert.DeserializeObject<Profile>(profileData);
            return profile.Data;
        }

        /// <summary>
        /// Sets the profile data.
        /// </summary>
        /// <param name="id">Profile identifier</param>
        /// <param name="data">Data blob</param>
        public void SetProfile(string id, string data)
        {
            var isValid = true; //TODO: change to false, just for testing
            var profilePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{RootPath}/{id}";

            if (!File.Exists(profilePath))
            {
                throw new FileNotFoundException();
            }

            var profileData = File.ReadAllText(profilePath);
            var profile =
                JsonConvert.DeserializeObject<Profile>(profileData);

            // TODO: verify data with stored public key

            if (isValid)
            {
                profile.Data = data;

                var profileJson = JsonConvert.SerializeObject(profile);

                File.WriteAllText(profilePath, profileJson);
            }
        }
    }
}