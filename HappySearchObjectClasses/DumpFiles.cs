using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using static Happy_Apps_Core.StaticHelpers;

namespace Happy_Apps_Core
{
    public static class DumpFiles
    {
        private const string TagsJsonGz = StoredDataFolder + "tags.json.gz";
        private const string TraitsJsonGz = StoredDataFolder + "traits.json.gz";
        private const string TagsJson = StoredDataFolder + "tags.json";
        private const string TraitsJson = StoredDataFolder + "traits.json";

        /// <summary>
        /// Contains all tags as in tags.json
        /// </summary>
        public static List<WrittenTag> PlainTags { get; private set; }

        /// <summary>
        /// Contains all traits as in traits.json
        /// </summary>
        public static List<WrittenTrait> PlainTraits { get; private set; }

        private static bool GetNewDumpFiles()
        {
            const int maxTries = 5;
            int tries = 0;
            bool complete = false;
            //tagdump section
            while (!complete && tries < maxTries)
            {
                if (File.Exists(TagsJsonGz)) continue;
                tries++;
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(TagsURL, TagsJsonGz);
                    }


                    GZipDecompress(TagsJsonGz, TagsJson);
                    File.Delete(TagsJsonGz);
                    complete = true;
                }
                catch (Exception e)
                {
                    LogToFile("GetNewDumpFiles Error", e);
                }
            }
            //load default file if new one couldnt be received or for some reason doesn't exist.
            if (!complete || !File.Exists(TagsJson)) LoadTagdump(true);
            else LoadTagdump();
            //traitdump section
            tries = 0;
            complete = false;
            while (!complete && tries < maxTries)
            {
                if (File.Exists(TraitsJsonGz)) continue;
                tries++;
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(TraitsURL, TraitsJsonGz);
                    }
                    GZipDecompress(TraitsJsonGz, TraitsJson);
                    File.Delete(TraitsJsonGz);
                    complete = true;
                }
                catch (Exception e)
                {
                    LogToFile("GetNewDumpFiles Error", e);
                }
            }
            //load default file if new one couldnt be received or for some reason doesn't exist.
            if (!complete || !File.Exists(TraitsJson))
            {
                LoadTraitdump(true);
                return false;
            }
            else
            {
                LoadTraitdump();
                return true;
            }
        }

        /// <summary>
        ///     Load Tags from Tag dump file.
        /// </summary>
        /// <param name="loadDefault">Load default file?</param>
        private static void LoadTagdump(bool loadDefault = false)
        {
            if (!loadDefault) loadDefault = !File.Exists(TagsJson);
            var fileToLoad = loadDefault ? DefaultTagsJson : TagsJson;
            LogToFile($"Attempting to load {fileToLoad}");
            try
            {
                PlainTags = JsonConvert.DeserializeObject<List<WrittenTag>>(File.ReadAllText(fileToLoad));
                List<ItemWithParents> baseList = PlainTags.Cast<ItemWithParents>().ToList();
                foreach (var writtenTag in PlainTags)
                {
                    writtenTag.SetItemChildren(baseList);
                }
            }
            catch (JsonReaderException e)
            {
                if (fileToLoad.Equals(DefaultTagsJson))
                {
                    //Should never happen.
                    LogToFile($"Failed to read default tags.json file, please download a new one from {TagsURL} uncompress it and paste it in {DefaultTagsJson}.");
                    PlainTags = new List<WrittenTag>();
                    return;
                }
                LogToFile($"{TagsJson} could not be read, deleting it and loading default tagdump.", e);
                File.Delete(TagsJson);
                LoadTagdump(true);
            }
        }

        /// <summary>
        ///     Load Traits from Trait dump file.
        /// </summary>
        private static void LoadTraitdump(bool loadDefault = false)
        {
            if (!loadDefault) loadDefault = !File.Exists(TraitsJson);
            var fileToLoad = loadDefault ? DefaultTraitsJson : TraitsJson;
            LogToFile($"Attempting to load {fileToLoad}");
            try
            {
                PlainTraits = JsonConvert.DeserializeObject<List<WrittenTrait>>(File.ReadAllText(fileToLoad));
                List<ItemWithParents> baseList = PlainTraits.Cast<ItemWithParents>().ToList();
                foreach (var writtenTrait in PlainTraits)
                {
                    writtenTrait.SetTopmostParent(PlainTraits);
                    writtenTrait.SetItemChildren(baseList);
                }
            }
            catch (JsonReaderException e)
            {
                if (fileToLoad.Equals(DefaultTraitsJson))
                {
                    //Should never happen.
                    LogToFile($"Failed to read default traits.json file, please download a new one from {TraitsURL} uncompress it and paste it in {DefaultTraitsJson}.");
                    PlainTraits = new List<WrittenTrait>();
                    return;
                }
                LogToFile($"{TraitsJson} could not be read, deleting it and loading default traitdump.", e);
                File.Delete(TraitsJson);
                LoadTraitdump(true);
            }
        }

        public static void Load()
        {
            if (DaysSince(Settings.DumpfileDate) > 2 || DaysSince(Settings.DumpfileDate) == -1)
            {
                if (!GetNewDumpFiles()) return;
                Settings.DumpfileDate = DateTime.UtcNow;
                Settings.Save();
            }
            else
            {
                //load dump files if they exist, otherwise load default.
                LoadTagdump();
                LoadTraitdump();
            }
        }
    }
}
