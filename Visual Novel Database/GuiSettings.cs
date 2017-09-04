using Newtonsoft.Json;
using System.IO;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Search
{
    class GuiSettings
    {
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
        /// Automatically log in using previously saved credentials.
        /// </summary>
        public bool RememberPassword { get; set; }
        
        /// <summary>
        /// Default constructor, sets all values to default.
        /// </summary>
        public GuiSettings()
        {
            ContentTags = true;
            SexualTags = false;
            TechnicalTags = true;
            NSFWImages = false;
            AutoUpdate = false;
            RememberPassword = false;
        }

        public static GuiSettings Load()
        {
            GuiSettings settings;
            if (!File.Exists(SettingsJson)) return new GuiSettings();
            try
            {
                settings = JsonConvert.DeserializeObject<GuiSettings>(File.ReadAllText(SettingsJson));
            }
            catch (JsonException exception)
            {
                LogToFile("UserSettings.Load Error", exception);
                return new GuiSettings();
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
                LogToFile("UserSettings.Save", exception);
            }
        }

    }
}
