using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;

namespace Happy_Search
{
    /// <summary>
    /// Static Methods
    /// </summary>
    public static class StaticHelpers
    {

        #region File Locations

#pragma warning disable 1591
        public const string TagsURL = "http://vndb.org/api/tags.json.gz";
        public const string TraitsURL = "http://vndb.org/api/traits.json.gz";
        public const string ProjectURL = "https://github.com/Zoltanar/Happy-Search";
        public const string DefaultTraitsJson = "Program Data\\Default Files\\traits.json";
        public const string DefaultTagsJson = "Program Data\\Default Files\\tags.json";

#if DEBUG
        public const string VNImagesFolder = "..\\Release\\Stored Data\\Saved Cover Images\\";
        public const string VNScreensFolder = "..\\Release\\Stored Data\\Saved Screenshots\\";
        public const string DBStatsXml = "..\\Release\\Stored Data\\dbs.xml";
        public const string MainXmlFile = "..\\Release\\Stored Data\\saved_objects.xml";
        public const string LogFile = "..\\Release\\Stored Data\\message.log";
        public const string TagsJsonGz = "..\\Release\\Stored Data\\tags.json.gz";
        public const string TraitsJsonGz = "..\\Release\\Stored Data\\traits.json.gz";
        public const string TagsJson = "..\\Release\\Stored Data\\tags.json";
        public const string TraitsJson = "..\\Release\\Stored Data\\traits.json";
#else
        public const string VNImagesFolder = "Stored Data\\Saved Cover Images\\";
        public const string VNScreensFolder = "Stored Data\\Saved Screenshots\\";
        public const string DBStatsXml = "Stored Data\\dbs.xml";
        public const string MainXmlFile = "Stored Data\\saved_objects.xml";
        public const string LogFile = "Stored Data\\message.log";
        public const string TagsJsonGz = "Stored Data\\tags.json.gz";
        public const string TraitsJsonGz = "Stored Data\\traits.json.gz";
        public const string TagsJson = "Stored Data\\tags.json";
        public const string TraitsJson = "Stored Data\\traits.json";
#endif
#pragma warning restore 1591

        #endregion

#pragma warning disable 1591
        public const string ContentTag = "cont";
        public const string SexualTag = "ero";
        public const string TechnicalTag = "tech";
        private const int LabelFadeTime = 5000;
        public static readonly Color SignalerDefault = Color.LightGray;
        public static readonly Color SignalerActive = Color.IndianRed;

        //tile background colors
        public static readonly SolidBrush DefaultTileBrush = new SolidBrush(Color.LightBlue);
        public static readonly SolidBrush WLHighBrush = new SolidBrush(Color.DeepPink);
        public static readonly SolidBrush WLMediumBrush = new SolidBrush(Color.Pink);
        public static readonly SolidBrush WLLowBrush = new SolidBrush(Color.LightPink);
        public static readonly SolidBrush ULFinishedBrush = new SolidBrush(Color.LightGreen);
        public static readonly SolidBrush ULStalledBrush = new SolidBrush(Color.DarkKhaki);
        public static readonly SolidBrush ULDroppedBrush = new SolidBrush(Color.DarkOrange);
        public static readonly SolidBrush ULUnknownBrush = new SolidBrush(Color.Gray);

        //tile text colors
        public static readonly SolidBrush FavoriteProducerBrush = new SolidBrush(Color.Yellow);
        public static readonly SolidBrush ULPlayingBrush = new SolidBrush(Color.Yellow);
        public static readonly SolidBrush UnreleasedBrush = new SolidBrush(Color.White);

        /// <summary>
        /// Categories of VN Tags
        /// </summary>
        public enum TagCategory { Content, Sexual, Technical }
#pragma warning restore 1591

        /// <summary>
        /// Print message to Debug and write it to log file.
        /// </summary>
        /// <param name="message">Message to be written</param>
        public static void LogToFile(string message)
        {
            Debug.Print(message);
            while (IsFileLocked(new FileInfo(LogFile)))
            {
                Thread.Sleep(25);
            }
            using (var writer = new StreamWriter(LogFile, true))
            {
                writer.WriteLine(message);
            }
        }

        /// <summary>
        /// Print exception to Debug and write it to log file.
        /// </summary>
        /// <param name="exception">Exception to be written to file</param>
        public static void LogToFile(Exception exception)
        {
            Debug.Print(exception.Message);
            Debug.Print(exception.StackTrace);
            while (IsFileLocked(new FileInfo(LogFile)))
            {
                Thread.Sleep(25);
            }
            using (var writer = new StreamWriter(LogFile, true))
            {
                writer.WriteLine(exception.Message);
                writer.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Check if file is locked,
        /// </summary>
        /// <param name="file">File to be checked</param>
        /// <returns>Whether file is locked</returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }
            return false;
        }

        /// <summary>
        ///     Writes message in a label with message text color.
        /// </summary>
        /// <param name="label">Label to which the message is written</param>
        /// <param name="message">Message to be written</param>
        /// <param name="disableFade"></param>
        public static void WriteText(Label label, string message, bool disableFade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = FormMain.NormalLinkColor;
            else label.ForeColor = FormMain.NormalColor;
            label.Text = message;
            if (disableFade) return;
            FadeLabel(label);
        }

        /// <summary>
        ///     Writes message in a label with warning text color.
        /// </summary>
        /// <param name="label">Label to which the message is written</param>
        /// <param name="message">Message to be written</param>
        /// <param name="fade">Should message disappear after a few seconds?</param>
        public static void WriteWarning(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = FormMain.WarningColor;
            else label.ForeColor = FormMain.WarningColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        /// <summary>
        ///     Writes message in a label with error text color.
        /// </summary>
        /// <param name="label">Label to which the message is written</param>
        /// <param name="message">Message to be written</param>
        /// <param name="fade">Should message disappear after a few seconds?</param>
        public static void WriteError(Label label, string message, bool fade = false)
        {
            var linkLabel = label as LinkLabel;
            if (linkLabel != null) linkLabel.LinkColor = FormMain.ErrorColor;
            else label.ForeColor = FormMain.ErrorColor;
            label.Text = message;
            if (fade) FadeLabel(label);
        }

        /// <summary>
        /// Convert number of bytes to human-readable formatted string, rounded to 1 decimal place. (e.g 79.4KB)
        /// </summary>
        /// <param name="byteCount">Number of bytes</param>
        /// <returns>Formatted string</returns>
        public static string BytesToString(int byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB" }; //int.MaxValue is in gigabyte range.
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num) + suf[place];
        }

        /// <summary>
        ///     Convert a string containing a date (in the format YYYY-MM-DD) to a DateTime.
        /// </summary>
        /// <param name="date">String to be converted</param>
        /// <returns>DateTime representing date in string</returns>
        public static DateTime StringToDate(string date)
        {
            //unreleased if date is null or doesnt have any digits (tba, n/a etc)
            if (date == null || !date.Any(char.IsDigit)) return DateTime.MaxValue;
            int[] dateArray = date.Split('-').Select(int.Parse).ToArray();
            var dtDate = new DateTime();
            var dateRegex = new Regex(@"^\d{4}-\d{2}-\d{2}$");
            if (dateRegex.IsMatch(date))
            {
                //handle possible invalid dates such as february 30
                var tryDone = false;
                var tryCount = 0;
                while (!tryDone)
                {
                    try
                    {
                        dtDate = new DateTime(dateArray[0], dateArray[1], dateArray[2] - tryCount);
                        tryDone = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        LogToFile(
                            $"Date: {dateArray[0]}-{dateArray[1]}-{dateArray[2] - tryCount} is invalid, trying again one day earlier");
                        tryCount++;
                    }
                }
                return dtDate;
            }
            //if date only has year-month, then if month hasn't finished = unreleased
            var monthRegex = new Regex(@"^\d{4}-\d{2}$");
            if (monthRegex.IsMatch(date))
            {
                dtDate = new DateTime(dateArray[0], dateArray[1], 28);
                return dtDate;
            }
            //if date only has year, then if year hasn't finished = unreleased
            var yearRegex = new Regex(@"^\d{4}$");
            if (yearRegex.IsMatch(date))
            {
                dtDate = new DateTime(dateArray[0], 12, 28);
                return dtDate;
            }
            return DateTime.MaxValue;
        }

        /// <summary>
        ///     Convert JSON-formatted string to list of tags.
        /// </summary>
        /// <param name="tagstring">JSON-formatted string</param>
        /// <returns>List of tags</returns>
        public static List<TagItem> StringToTags(string tagstring)
        {
            if (tagstring.Equals("")) return new List<TagItem>();
            var curS = $"{{\"tags\":{tagstring}}}";
            var vnitem = JsonConvert.DeserializeObject<VNItem>(curS);
            return vnitem.Tags;
        }

        /// <summary>
        /// Get number of tags in specified category.
        /// </summary>
        /// <param name="tags">Collection of tags</param>
        /// <param name="category">Category that tags should be in</param>
        /// <returns>Number of tags that match</returns>
        public static int GetTagCountByCat(this IEnumerable<TagItem> tags, TagCategory category)
        {
            switch (category)
            {
                case TagCategory.Content:
                    return tags.Count(tag => tag.Category == TagCategory.Content);
                case TagCategory.Sexual:
                    return tags.Count(tag => tag.Category == TagCategory.Sexual);
                case TagCategory.Technical:
                    return tags.Count(tag => tag.Category == TagCategory.Technical);
            }
            return 1;
        }

        /// <summary>
        ///     Decompress GZip file.
        /// </summary>
        /// <param name="fileToDecompress">File to Decompress</param>
        /// <param name="outputFile">Output File</param>
        public static void GZipDecompress(string fileToDecompress, string outputFile)
        {
            using (var originalFileStream = File.OpenRead(fileToDecompress))
            {
                var newFileName = outputFile;

                using (var decompressedFileStream = File.Create(newFileName))
                {
                    using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        LogToFile($@"Decompressed: {fileToDecompress}");
                    }
                }
            }
        }

        /// <summary>
        ///     Get Days passed since date of last update.
        /// </summary>
        /// <param name="updatedDate">Date of last update</param>
        /// <returns>Number of days since last update</returns>
        public static int DaysSince(DateTime updatedDate)
        {
            if (updatedDate == DateTime.MinValue) return -1;
            var days = (DateTime.UtcNow - updatedDate).Days;
            return days;
        }

        /// <summary>
        /// Truncates strings if they are longer than 'maxChars'.
        /// </summary>
        /// <param name="value">The string to be truncated</param>
        /// <param name="maxChars">The maximum number of characters</param>
        /// <returns>Truncated string</returns>
        public static string TruncateString(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        /// <summary>
        ///     Convert DateTime to UnixTimestamp.
        /// </summary>
        /// <param name="dateTime">DateTime to be converted</param>
        /// <returns>UnixTimestamp (double)</returns>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        ///     Saves a VN's cover image (unless it already exists)
        /// </summary>
        /// <param name="vn">VN whose image is to be saved</param>
        /// <param name="update">Get new image regardless of whether one already exists?</param>
        public static void SaveImage(VNItem vn, bool update = false)
        {
            if (!Directory.Exists(VNImagesFolder)) Directory.CreateDirectory(VNImagesFolder);
            if (vn.Image == null || vn.Image.Equals("")) return;
            string imageLocation = $"{VNImagesFolder}{vn.ID}{Path.GetExtension(vn.Image)}";
            if (File.Exists(imageLocation) && update == false) return;
            LogToFile($"Start downloading cover image for {vn}");
            try
            {
                var requestPic = WebRequest.Create(vn.Image);
                using (var responsePic = requestPic.GetResponse())
                {
                    using (var stream = responsePic.GetResponseStream())
                    {
                        if (stream == null) return;
                        var webImage = Image.FromStream(stream);
                        webImage.Save(imageLocation);
                    }
                }
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentNullException || ex is SecurityException || ex is UriFormatException)
            {
                LogToFile(ex);
            }
        }

        /// <summary>
        /// Saves screenshot (by URL) to specified location.
        /// </summary>
        /// <param name="imageUrl">URL of image</param>
        /// <param name="savedLocation">Location to save image to</param>
        public static void SaveScreenshot(string imageUrl, string savedLocation)
        {
            if (!Directory.Exists(VNScreensFolder)) Directory.CreateDirectory(VNScreensFolder);
            string[] urlSplit = imageUrl.Split('/');
            if (!Directory.Exists($"{VNScreensFolder}\\{urlSplit[urlSplit.Length - 2]}"))
                Directory.CreateDirectory($"{VNScreensFolder}\\{urlSplit[urlSplit.Length - 2]}");
            if (imageUrl.Equals("")) return;
            if (File.Exists(savedLocation)) return;
            var requestPic = WebRequest.Create(imageUrl);
            using (var responsePic = requestPic.GetResponse())
            {
                using (var stream = responsePic.GetResponseStream())
                {
                    if (stream == null) return;
                    var webImage = Image.FromStream(stream);
                    webImage.Save(savedLocation);
                }
            }
        }

        /// <summary>
        ///     Delete text in label after time set in LabelFadeTime.
        /// </summary>
        /// <param name="tLabel">Label to delete text in</param>
        public static void FadeLabel(Label tLabel)
        {
            var fadeTimer = new Timer { Interval = LabelFadeTime };
            fadeTimer.Tick += (sender, e) =>
            {
                tLabel.Text = "";
                fadeTimer.Stop();
            };
            fadeTimer.Start();
        }

        /// <summary>
        ///     Saves a title's cover image (unless it already exists)
        /// </summary>
        /// <param name="vn">Title</param>
        public static async Task SaveImageAsync(ListedVN vn)
        {
            if (!Directory.Exists(VNImagesFolder)) Directory.CreateDirectory(VNImagesFolder);
            if (vn.ImageURL == null || vn.ImageURL.Equals("")) return;
            string imageLocation = $"{VNImagesFolder}{vn.VNID}{Path.GetExtension(vn.ImageURL)}";
            if (File.Exists(imageLocation)) return;
            LogToFile($"Start downloading cover image for {vn}");
            try
            {
                var requestPic = WebRequest.Create(vn.ImageURL);
                using (var responsePic = await requestPic.GetResponseAsync())
                {
                    using (var stream = responsePic.GetResponseStream())
                    {
                        if (stream == null) return;
                        var webImage = Image.FromStream(stream);
                        webImage.Save(imageLocation);
                    }
                }
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentNullException || ex is SecurityException || ex is UriFormatException)
            {
                LogToFile(ex);
            }
        }

        /// <summary>
        ///     Check if date is in the future.
        /// </summary>
        /// <param name="date">Date to be checked</param>
        /// <returns>Whether date is in the future</returns>
        public static bool DateIsUnreleased(string date)
        {
            return StringToDate(date) > DateTime.UtcNow;
        }
    }
}
