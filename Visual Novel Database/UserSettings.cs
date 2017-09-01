using System;
using Newtonsoft.Json;
using System.IO;
using static Happy_Search.StaticHelpers;

namespace Happy_Search
{
    class UserSettings
    {
        /// <summary>
        /// Username of user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// VNDB's UserID for user (found in the user's profile page).
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Show content tags.
        /// </summary>
        public bool ContentTags { get; set; }

        /// <summary>
        /// Show sexual content tags.
        /// </summary>
        public bool SexualTags { get; set; }

        /// <summary>
        /// Show technical tags.
        /// </summary>
        public bool TechnicalTags { get; set; }

        /// <summary>
        /// Show Not-safe-for-work images.
        /// </summary>
        public bool NSFWImages { get; set; }

        /// <summary>
        /// Update user's user-related titles (URT) automatically upon loading program.
        /// </summary>
        public bool AutoUpdate { get; set; }

        /// <summary>
        /// Don't get titles released over a decade ago (does not apply to searched by name or favorite producer titles).
        /// </summary>
        public bool DecadeLimit { get; set; }

        /// <summary>
        /// Automatically log in using previously saved credentials.
        /// </summary>
        public bool RememberPassword { get; set; }

        /// <summary>
        /// Date of last time that dump files were downloaded.
        /// </summary>
        public DateTime DumpfileDate { get; set; }

        /// <summary>
        /// Date of last time that VNDB Stats were fetched.
        /// </summary>
        public DateTime StatsDate { get; set; }

        /// <summary>
        /// Date of last time that User's User-related titles (URT) were fetched.
        /// </summary>
        public DateTime URTDate { get; set; }
        
        /// <summary>
        /// Default constructor, sets all values to default.
        /// </summary>
        public UserSettings()
        {
            Username = "";
            UserID = -1;
            ContentTags = true;
            SexualTags = false;
            TechnicalTags = true;
            NSFWImages = false;
            AutoUpdate = false;
            DecadeLimit = true;
            RememberPassword = false;
            DumpfileDate = DateTime.MinValue;
            StatsDate = DateTime.MinValue;
            URTDate = DateTime.MinValue;
        }

        public static UserSettings Load()
        {
            UserSettings settings;
            if (!File.Exists(SettingsJson)) return new UserSettings();
            try
            {
                settings = JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(SettingsJson));
            }
            catch (JsonException exception)
            {
                LogToFile(exception);
                return new UserSettings();
            }
            return settings;
        }


        public void Save()
        {
            try
            {
                File.WriteAllText(SettingsJson, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (JsonException exception)
            {
                LogToFile(exception);
            }
        }

    }
}
