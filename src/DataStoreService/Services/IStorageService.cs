namespace DataStoreService.Services
{
    public interface IStorageService
    {
        /// <summary>
        /// Creates a new profile storage.
        /// </summary>
        /// <param name="key">Public key value</param>
        /// <param name="data">Data blob</param>
        /// <returns></returns>
        string CreateProfile(Profile data);

        /// <summary>
        /// Gets the profile for a given identifier.
        /// </summary>
        /// <param name="id">Profile identifier</param>
        /// <returns>Data in a string profile</returns>
        string GetProfile(string id);

        /// <summary>
        /// Sets the profile data.
        /// </summary>
        /// <param name="id">Profile identifier</param>
        /// <param name="data">Data blob</param>
        void SetProfile(string id, string data);
    }
}