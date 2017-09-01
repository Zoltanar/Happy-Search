using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static Happy_Search.FormMain;
using static Happy_Search.StaticHelpers;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162


namespace Happy_Search
{
    internal class DbHelper
    {
        #region Initialization
#if DEBUG
        private const string DbFile = "..\\Release\\Stored Data\\Happy-Search-Local-DB.sqlite";
#else
        private const string DbFile = "Stored Data\\Happy-Search-Local-DB.sqlite";
#endif

        private const string DbConnectionString = "Data Source=" + DbFile + ";Version=3;";
        private readonly SQLiteConnection _conn;
        private static bool _dbLog;

        public DbHelper(bool dbLog = false)
        {
            _dbLog = dbLog;
            _printSetMethods = dbLog;
            _printGetMethods = dbLog;

            _conn = new SQLiteConnection(DbConnectionString);
            InitDatabase();

        }


        private void InitDatabase()
        {
            if (File.Exists(DbFile))
            {
                //check database version and update if necessary.
                Open();
                var version = GetCurrentVersion();
                if (version < DatabaseVersion.Latest)
                {
                    if (_dbLog) LogToFile("Updating Database");
                    UpgradeDatabase(version);
                    if (_dbLog) LogToFile("Finished Updating Database");
                }
                Close();
                return;
            }
            SQLiteConnection.CreateFile(DbFile);
            if (_dbLog) LogToFile("Creating Database");
            BeginTransaction();
            //must be in this order
            CreateProducerListTable();
            CreateVNListTable();
            CreateCharacterListTable();
            CreateUserlistTable();
            CreateUserProdListTable();
            CreateTableDetails();
            CreateTriggers();
            EndTransaction();
            if (_dbLog) LogToFile("Finished Creating Database");
        }

        private DatabaseVersion GetCurrentVersion()
        {
            //check if table exists
            var selectString = "SELECT name FROM sqlite_master WHERE type='table' AND name='tabledetails';";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var returned = command.ExecuteScalar();
            if (returned == null) return DatabaseVersion.Pre;
            //check table version
            selectString = "SELECT Value FROM tabledetails WHERE Key='databaseversion';";
            if (_printGetMethods) LogToFile(selectString);
            command = new SQLiteCommand(selectString, _conn);
            var version = command.ExecuteScalar().ToString();
            switch (version)
            {
                case "V1_4_0":
                    return DatabaseVersion.V1_4_0;
                case "V1_4_7":
                    return DatabaseVersion.V1_4_7;
                default:
                    return DatabaseVersion.Pre;
            }
        }

        private void UpgradeDatabase(DatabaseVersion current)
        {
            try
            {
                BackupPreviousDatabase(current);
            }
            catch (Exception ex) when (ex is PathTooLongException || ex is UnauthorizedAccessException)
            {
                LogToFile("Couldn't backup previous version.",ex);
            }
            if (current < DatabaseVersion.V1_4_0)
            {
                //remove update producerlist date trigger
                var commandString = "DROP TRIGGER UpdateTimestampProducerList;";
                if (_printGetMethods) LogToFile(commandString);
                var command = new SQLiteCommand(commandString, _conn);
                command.ExecuteNonQuery();
                //set Updated values to null
                commandString = "UPDATE producerlist SET Updated = NULL;";
                if (_printGetMethods) LogToFile(commandString);
                command = new SQLiteCommand(commandString, _conn);
                command.ExecuteNonQuery();
                //create tabledetails table
                CreateTableDetails();
            }
            if (current < DatabaseVersion.V1_4_7)
            {
                //Add columns to vnlist table (aliases, languages)
                var commandString = @"ALTER TABLE vnlist
  ADD Aliases TEXT;
ALTER TABLE vnlist
 ADD Languages TEXT;
ALTER TABLE vnlist
 ADD DateFullyUpdated DATE;";
                if (_printGetMethods) LogToFile(commandString);
                var command = new SQLiteCommand(commandString, _conn);
                command.ExecuteNonQuery();
                commandString = @"ALTER TABLE producerlist ADD Language TEXT;";
                if (_printGetMethods) LogToFile(commandString);
                command = new SQLiteCommand(commandString, _conn);
                command.ExecuteNonQuery();
            }
            SetDbVersion(DatabaseVersion.Latest);
        }

        private void BackupPreviousDatabase(DatabaseVersion previous)
        {
            if (File.Exists(DbFile))
            {
                var extLength = Path.GetExtension(DbFile).Length;
                var backupFile = $"{DbFile.Substring(0, DbFile.Length - extLength)} - {previous} Backup.sqlite";
                if (File.Exists(backupFile)) File.Delete(backupFile);
                File.Copy(DbFile, backupFile);
            }
        }

        private void CreateTableDetails()
        {
            const string createCommand =
                @"CREATE TABLE `tabledetails` (
	`Key`	TEXT NOT NULL,
	`Value`	TEXT,
	PRIMARY KEY(`Key`)
);";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
            string insertCommand = @"INSERT INTO tabledetails  (Key,Value) VALUES ('databaseversion','V1_4_7');";
            command = new SQLiteCommand(insertCommand, _conn);
            command.ExecuteNonQuery();
            insertCommand = @"INSERT INTO tabledetails  (Key,Value) VALUES ('programname','Happy Search');";
            command = new SQLiteCommand(insertCommand, _conn);
            command.ExecuteNonQuery();
            insertCommand = @"INSERT INTO tabledetails  (Key,Value) VALUES ('author','zoltanar');";
            command = new SQLiteCommand(insertCommand, _conn);
            command.ExecuteNonQuery();
            insertCommand = $@"INSERT INTO tabledetails  (Key,Value) VALUES ('projecturl','{ProjectURL}');";
            command = new SQLiteCommand(insertCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void SetDbVersion(DatabaseVersion version)
        {
            string versionString;
            switch (version)
            {
                case DatabaseVersion.V1_4_0:
                    versionString = "V1_4_0";
                    break;
                case DatabaseVersion.V1_4_7:
                    versionString = "V1_4_7";
                    break;
                default:
                    return;
            }
            string insertCommand = $@"INSERT OR REPLACE INTO tabledetails  (Key,Value) VALUES ('databaseversion','{versionString}');";
            var command = new SQLiteCommand(insertCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void CreateVNListTable()
        {
            const string createCommand = @"CREATE TABLE ""vnlist"" ( 
`VNID` INTEGER NOT NULL UNIQUE, 
`Title` TEXT, 
`KanjiTitle` TEXT, 
`RelDate` TEXT, 
`ProducerID` INTEGER, 
`Tags` TEXT, 
`DateUpdated` DATE DEFAULT CURRENT_TIMESTAMP, 
`ImageURL` TEXT, 
`ImageNSFW` INTEGER, 
`Description` TEXT, 
`LengthTime` INTEGER, 
`Popularity` NUMERIC, 
`Rating` NUMERIC, 
`VoteCount` INTEGER, 
`Relations` TEXT, 
`Screens` TEXT, 
`Anime` TEXT, 
`Aliases` TEXT, 
`Languages` TEXT, 
PRIMARY KEY(`VNID`), 
FOREIGN KEY(`ProducerID`) REFERENCES `ProducerID` )";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void CreateProducerListTable()
        {
            const string createCommand = @"CREATE TABLE `producerlist` (
	`ProducerID`	INTEGER NOT NULL,
	`Name`	TEXT,
	`Titles`	INTEGER,
	`Loaded`	TEXT,
	`Updated`	DATE DEFAULT CURRENT_TIMESTAMP,
	`Language`	TEXT,
	PRIMARY KEY(`ProducerID`)
);";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void CreateCharacterListTable()
        {
            var createCommand = @"CREATE TABLE ""charlist"" ( 
`CharacterID` INTEGER NOT NULL UNIQUE, 
`Name` TEXT, 
`Image` TEXT, 
`Traits` TEXT, 
`VNs` TEXT, 
`DateUpdated` INTEGER, 
PRIMARY KEY(`CharacterID`) )";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void CreateUserlistTable()
        {
            const string createCommand = @"CREATE TABLE `userlist` (
 `VNID`	INTEGER,
 `UserID`	INTEGER,
 `ULStatus`	INTEGER,
 `ULAdded`	INTEGER,
 `ULNote`	TEXT,
 `WLStatus`	INTEGER,
 `WLAdded`	INTEGER,
 `Vote`	INTEGER,
 `VoteAdded`	INTEGER,
 PRIMARY KEY(VNID,UserID)
)";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void CreateUserProdListTable()
        {
            const string createCommand = @"CREATE TABLE `userprodlist`(
	`ProducerID`	INTEGER NOT NULL,
	`UserID`	INTEGER NOT NULL,
	`UserAverageVote`	NUMERIC,
	`UserDropRate`	INTEGER,
	PRIMARY KEY(ProducerID, UserID)
)";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
        }

        private void CreateTriggers()
        {
            const string createCommand = @"CREATE TRIGGER [UpdateTimestamp]
    AFTER UPDATE    ON vnlist    FOR EACH ROW
BEGIN
    UPDATE vnlist 
	SET DateUpdated=CURRENT_TIMESTAMP
	WHERE VNID=OLD.VNID;
END";
            const string createCommand3 = @"CREATE TRIGGER [UpdateTimestampCharacterList] 
AFTER UPDATE ON charlist FOR EACH ROW 
BEGIN 
UPDATE charlist SET DateUpdated=CURRENT_TIMESTAMP 
WHERE CharacterID=OLD.CharacterID; 
END";
            var command = new SQLiteCommand(createCommand, _conn);
            command.ExecuteNonQuery();
            var command3 = new SQLiteCommand(createCommand3, _conn);
            command3.ExecuteNonQuery();
        }

        #endregion

        #region Set Methods

        private static bool _printSetMethods;

        public void AddNoteToVN(int vnid, string note, int userID)
        {
            var noteString = Regex.Replace(note, "'", "''");
            var insertString =
                $"UPDATE userlist SET ULNote = '{noteString}' WHERE VNID = {vnid} AND UserID = {userID};";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }
        public void AddRelationsToVN(int vnid, RelationsItem[] relations)
        {
            var relationsString = relations.Any() ? ListToJsonArray(new List<object>(relations)) : "Empty";
            relationsString = Regex.Replace(relationsString, "'", "''");
            var insertString =
                $"UPDATE vnlist SET Relations = '{relationsString}' WHERE VNID = {vnid};";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }

        public void AddScreensToVN(int vnid, ScreenItem[] screens)
        {
            var screensString = screens.Any() ? ListToJsonArray(new List<object>(screens)) : "Empty";
            var insertString =
                $"UPDATE vnlist SET Screens = '{screensString}' WHERE VNID = {vnid};";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }

        public void AddAnimeToVN(int vnid, AnimeItem[] anime)
        {
            var animeString = anime.Any() ? ListToJsonArray(new List<object>(anime)) : "Empty";
            animeString = Regex.Replace(animeString, "'", "''");
            var insertString =
                $"UPDATE vnlist SET Anime = '{animeString}' WHERE VNID = {vnid};";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }

        public void UpdateVNTagsStats(VNItem vnItem)
        {
            var tags = ListToJsonArray(new List<object>(vnItem.Tags));
            var insertString =
                $"UPDATE vnlist SET Tags = '{tags}', Popularity = {vnItem.Popularity.ToString("0.00", CultureInfo.InvariantCulture)}, Rating = {vnItem.Rating.ToString("0.00", CultureInfo.InvariantCulture)}, VoteCount = {vnItem.VoteCount} WHERE VNID = {vnItem.ID};";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }

        public void UpdateVNStatus(int userID, int vnid, ChangeType type, int statusInt, Command command, double newVoteValue = -1)
        {
            if (command == Command.Delete)
            {
                string deleteString = $"DELETE FROM userlist WHERE VNID = {vnid} AND UserID = {userID};";
                if (_printSetMethods) LogToFile(deleteString);
                var cmd = new SQLiteCommand(deleteString, _conn);
                cmd.ExecuteNonQuery();
                return;
            }
            var commandString = "";
            var statusDate = statusInt == -1
                ? "NULL"
                : DateTimeToUnixTimestamp(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture);
            switch (type)
            {
                case ChangeType.UL:
                    switch (command)
                    {
                        case Command.New:
                            commandString =
                                $"INSERT OR REPLACE INTO userlist (VNID, UserID, ULStatus, ULAdded) Values ({vnid}, {userID}, {statusInt}, {statusDate});";
                            break;
                        case Command.Update:
                            commandString =
                                $"UPDATE userlist SET ULStatus = {statusInt}, ULAdded = {statusDate} WHERE VNID = {vnid} AND UserID = {userID};";
                            break;
                    }
                    break;
                case ChangeType.WL:
                    switch (command)
                    {
                        case Command.New:
                            commandString =
                                $"INSERT OR REPLACE  INTO userlist (VNID, UserID, WLStatus, WLAdded) Values ({vnid}, {userID}, {statusInt}, {statusDate});";
                            break;
                        case Command.Update:
                            commandString =
                                $"UPDATE userlist SET WLStatus = {statusInt}, WLAdded = {statusDate} WHERE VNID = {vnid} AND UserID = {userID};";
                            break;
                    }
                    break;
                case ChangeType.Vote:
                    int vote = (int)Math.Abs(newVoteValue * 10);
                    switch (command)
                    {
                        case Command.New:
                            commandString =
                                "INSERT OR REPLACE  INTO userlist (VNID, UserID, Vote, VoteAdded) " +
                                $"Values ({vnid}, {userID}, {vote}, {statusDate});";
                            break;
                        case Command.Update:
                            commandString =
                                $"UPDATE userlist SET Vote = {(statusInt != -1 ? vote.ToString() : "NULL")}, VoteAdded = {statusDate} WHERE VNID = {vnid} AND UserID = {userID};";
                            break;
                    }
                    break;
            }
            if (commandString.Equals("")) return;
            if (_printSetMethods) LogToFile(commandString);
            var cmd2 = new SQLiteCommand(commandString, _conn);
            cmd2.ExecuteNonQuery();
        }

        public void InsertFavoriteProducers(List<ListedProducer> addProducerList, int userid)
        {
            foreach (var item in addProducerList)
            {
                var insertString =
                    $"INSERT OR REPLACE INTO userprodlist (ProducerID, UserID, UserAverageVote, UserDropRate) VALUES ({item.ID}, {userid}, {item.UserAverageVote.ToString("0.00", CultureInfo.InvariantCulture)}, {item.UserDropRate});";
                var command = new SQLiteCommand(insertString, _conn);
                if (_printSetMethods) LogToFile(insertString);
                command.ExecuteNonQuery();
            }
        }


        internal bool IsBusy()
        {
            return _conn.State != ConnectionState.Closed;
        }

        /// <summary>
        /// Insert or Replace Producer into producerlist, if adding for the first time, set date to null.
        /// </summary>
        /// <param name="producer">The producer to be inserted</param>
        /// <param name="setDateNull">Sets date to null rather than the default (which is CURRENT TIMESTAMP)</param>
        public void InsertProducer(ListedProducer producer, bool setDateNull = false)
        {
            var name = Regex.Replace(producer.Name, "'", "''");
            var commandString = setDateNull ?
                $"INSERT OR REPLACE INTO producerlist (ProducerID, Name, Language, Titles, Updated) VALUES ({producer.ID}, '{name}', '{producer.Language}', {producer.NumberOfTitles}, NULL);" :
                $"INSERT OR REPLACE INTO producerlist (ProducerID, Name, Language, Titles) VALUES ({producer.ID}, '{name}', '{producer.Language}', {producer.NumberOfTitles});";
            if (_printSetMethods) LogToFile(commandString);
            var cmd = new SQLiteCommand(commandString, _conn);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds or Updates all titles in User-related title list.
        /// </summary>
        /// <param name="userid">ID of User</param>
        /// <param name="urtList">List of URT titles</param>
        public void UpdateURTTitles(int userid, IEnumerable<UrtListItem> urtList)
        {
            foreach (var item in urtList)
            {
                var comm = new SQLiteCommand(_conn);
                switch (item.Action)
                {
                    case Command.New:
                    case Command.Update:
                        comm.CommandText =
                            "INSERT OR REPLACE INTO userlist (VNID, UserID, ULStatus, ULAdded, ULNote, WLStatus, WLAdded, Vote, VoteAdded)" +
                            "VALUES (@id, @uid, @uls, @ula, @uln, @wls, @wla, @vo, @va);";
                        comm.Parameters.Add(new SQLiteParameter("@uls", item.ULStatus));
                        comm.Parameters.Add(new SQLiteParameter("@ula", item.ULAdded));
                        comm.Parameters.Add(new SQLiteParameter("@uln", item.ULNote));
                        comm.Parameters.Add(new SQLiteParameter("@wls", item.WLStatus));
                        comm.Parameters.Add(new SQLiteParameter("@wla", item.WLAdded));
                        comm.Parameters.Add(new SQLiteParameter("@vo", item.Vote));
                        comm.Parameters.Add(new SQLiteParameter("@va", item.VoteAdded));
                        break;
                    case Command.Delete:
                        comm.CommandText = "DELETE FROM userlist WHERE VNID = @id AND UserID = @uid";
                        break;
                }
                comm.Parameters.Add(new SQLiteParameter("@id", item.ID));
                comm.Parameters.Add(new SQLiteParameter("@uid", userid));
                comm.ExecuteNonQuery();
            }
        }

        public void UpsertSingleCharacter(CharacterItem character)
        {
            var insertString =
                $"INSERT OR REPLACE INTO charlist (CharacterID, Traits, VNs) VALUES ('{character.ID}', '{ListToJsonArray(new List<object>(character.Traits))}','{ListToJsonArray(new List<object>(character.VNs))}');";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }

        public void UpsertSingleVN((VNItem item, ProducerItem producer, VNLanguages languages) data, bool setFullyUpdated)
        {
            var (item, producer, languages) = data;
            var tags = ListToJsonArray(new List<object>(item.Tags));
            var command = new SQLiteCommand(_conn);
            if (setFullyUpdated)
            {
                command.CommandText = "INSERT OR REPLACE INTO vnlist (" +
                   "Title, KanjiTitle, ProducerID, RelDate, Tags, Description, " +
                   "ImageURL, ImageNSFW, LengthTime, " +
                   "Popularity, Rating, " +
                   "VoteCount, VNID, Aliases, Languages, DateFullyUpdated)" +
                   "VALUES(" +
                   "@title, @kanjititle, @producerid, @reldate, @tags, @description, " +
                   "@imageurl, @imagensfw, @lengthtime, " +
                   "@popularity,@rating, " +
                   "@votecount, @vnid, @aliases, @languages, @datefullyupdated);";
                command.Parameters.Add(new SQLiteParameter("@datefullyupdated", DateTime.UtcNow));
            }
            else
            {
                command.CommandText = "INSERT OR REPLACE INTO vnlist (" +
                   "Title, KanjiTitle, ProducerID, RelDate, Tags, Description, " +
                   "ImageURL, ImageNSFW, LengthTime, " +
                   "Popularity, Rating, " +
                   "VoteCount, VNID, Aliases, Languages)" +
                   "VALUES(" +
                   "@title, @kanjititle, @producerid, @reldate, @tags, @description, " +
                   "@imageurl, @imagensfw, @lengthtime, " +
                   "@popularity,@rating, " +
                   "@votecount, @vnid, @aliases, @languages);";
            }
            command.Parameters.Add(new SQLiteParameter("@title", item.Title));
            command.Parameters.Add(new SQLiteParameter("@kanjititle", item.Original));
            command.Parameters.Add(new SQLiteParameter("@producerid", producer?.ID ?? -1));
            command.Parameters.Add(new SQLiteParameter("@reldate", item.Released));
            command.Parameters.Add(new SQLiteParameter("@tags", tags));
            command.Parameters.Add(new SQLiteParameter("@description", item.Description));
            command.Parameters.Add(new SQLiteParameter("@imageurl", item.Image));
            command.Parameters.Add(new SQLiteParameter("@imagensfw", item.Image_Nsfw));
            command.Parameters.Add(new SQLiteParameter("@lengthtime", item.Length));
            command.Parameters.Add(new SQLiteParameter("@popularity", item.Popularity));
            command.Parameters.Add(new SQLiteParameter("@rating", item.Rating));
            command.Parameters.Add(new SQLiteParameter("@votecount", item.VoteCount));
            command.Parameters.Add(new SQLiteParameter("@vnid", item.ID));
            command.Parameters.Add(new SQLiteParameter("@aliases", item.Aliases));
            command.Parameters.Add(new SQLiteParameter("@languages", languages?.ToString()));
            if (_printSetMethods) LogToFile(command.CommandText);
            command.ExecuteNonQuery();
        }

        public void SetProducerLanguage(ListedProducer producer)
        {
            var insertString =
                $"UPDATE producerlist SET Language = '{producer.Language}' WHERE ProducerID = {producer.ID};";
            if (_printSetMethods) LogToFile(insertString);
            var command = new SQLiteCommand(insertString, _conn);
            command.ExecuteNonQuery();
        }

        public void RemoveFavoriteProducer(int producerID, int userid)
        {
            var commandString = $"DELETE FROM userprodlist WHERE ProducerID={producerID} AND UserID={userid};";
            if (_printSetMethods) LogToFile(commandString);
            var command = new SQLiteCommand(commandString, _conn);
            command.ExecuteNonQuery();
        }

        public void RemoveVisualNovel(int vnid)
        {
            var commandString = $"DELETE FROM vnlist WHERE VNID={vnid};";
            if (_printSetMethods) LogToFile(commandString);
            var command = new SQLiteCommand(commandString, _conn);
            command.ExecuteNonQuery();
        }

        #endregion

        #region Get Methods

        private static bool _printGetMethods;

        public List<int> GetUnfetchedUserRelatedTitles(int userid)
        {
            var selectString =
                $"SELECT VNID FROM userlist WHERE VNID NOT IN (SELECT VNID FROM vnlist) AND UserID = {userid};";
            var list = new List<int>();
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(DbInt(reader["VNID"]));
            return list;
        }

        public ListedVN GetSingleVN(int vnid, int userid)
        {
            var selectString =
                $"SELECT vnlist.*, userlist.*, producerlist.Name FROM vnlist LEFT JOIN userlist ON vnlist.VNID = userlist.VNID AND userlist.UserID ={userid} LEFT JOIN producerlist ON producerlist.ProducerID = vnlist.ProducerID WHERE vnlist.VNID={vnid};";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            ListedVN vn = null;
            while (reader.Read()) vn = new ListedVN(reader);
            return vn;
        }

        public List<ListedProducer> GetAllProducers()
        {
            var list = new List<ListedProducer>();
            var selectString = "SELECT * FROM producerlist;";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(GetListedProducer(reader));
            }
            return list;
        }

        public List<ListedProducer> GetFavoriteProducersForUser(int userid)
        {
            var readerList = new List<ListedProducer>();
            var selectString =
                $"SELECT producerlist.*, userprodlist.UserAverageVote, userprodlist.UserDropRate FROM producerlist LEFT JOIN userprodlist ON producerlist.ProducerID = userprodlist.ProducerID WHERE userprodlist.UserID = {userid};";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(GetFavoriteProducer(reader));
            return readerList;
        }

        public List<ListedVN> GetUserRelatedTitles(int userid)
        {
            var readerList = new List<ListedVN>();
            var selectString =
                $"SELECT vnlist.*, userlist.*, producerlist.Name FROM vnlist, userlist  LEFT JOIN producerlist ON producerlist.ProducerID = vnlist.ProducerID WHERE vnlist.VNID = userlist.VNID AND UserID = {userid};";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(new ListedVN(reader));
            return readerList;
        }

        public List<ListedVN> GetAllTitles(int userid)
        {
            var readerList = new List<ListedVN>();
            var selectString =
                $"SELECT vnlist.*, userlist.*, producerlist.Name FROM vnlist LEFT JOIN userlist ON vnlist.VNID = userlist.VNID AND userlist.UserID ={userid} LEFT JOIN producerlist ON producerlist.ProducerID = vnlist.ProducerID WHERE Title NOT NULL;";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(new ListedVN(reader));
            return readerList;
        }

        public List<CharacterItem> GetAllCharacters()
        {
            var readerList = new List<CharacterItem>();
            var selectString = "SELECT * FROM charlist;";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(GetCharacterItem(reader));
            return readerList;
        }

        /// <summary>
        ///     Get titles developed by Producer given.
        /// </summary>
        /// <param name="userID">ID of current user</param>
        /// <param name="producerID">ID of producer</param>
        /// <returns>List of titles by given producer</returns>
        public List<ListedVN> GetTitlesFromProducerID(int userID, int producerID)
        {
            var readerList = new List<ListedVN>();
            var selectString =
                $"SELECT * FROM vnlist LEFT JOIN producerlist ON vnlist.ProducerID = producerlist.ProducerID LEFT JOIN userlist ON vnlist.VNID = userlist.VNID AND userlist.UserID={userID} WHERE vnlist.ProducerID={producerID};";
            if (_printGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, _conn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(new ListedVN(reader));
            return readerList;
        }

        #endregion

        #region Other

        private static CharacterItem GetCharacterItem(SQLiteDataReader reader)
        {
            return new CharacterItem
            {
                ID = DbInt(reader["CharacterID"]),
                Traits = JsonConvert.DeserializeObject<List<TraitItem>>(reader["Traits"].ToString()),
                VNs = JsonConvert.DeserializeObject<List<CharacterVNItem>>(reader["VNs"].ToString())
            };
        }
        
        private static ListedProducer GetListedProducer(SQLiteDataReader reader)
        {
            return new ListedProducer(
                reader["Name"].ToString(),
                DbInt(reader["Titles"]),
                DbDateTime(reader["Updated"]),
                DbInt(reader["ProducerID"]),
                reader["Language"].ToString());
        }

        private static ListedProducer GetFavoriteProducer(SQLiteDataReader reader)
        {
            return new ListedProducer(
                reader["Name"].ToString(),
                DbInt(reader["Titles"]),
                DbDateTime(reader["Updated"]),
                DbInt(reader["ProducerID"]),
                reader["Language"].ToString(),
                DbDouble(reader["UserAverageVote"]),
                DbInt(reader["UserDropRate"]));
        }

        public void Open()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
                if (_dbLog) LogToFile("Opened Database");
            }
            else
            {
                LogToFile("Tried to open database, it was already opened.");
            }
        }

        public void Close()
        {
            _conn.Close();
            if (_dbLog) LogToFile("Closed Database");
        }

        private SQLiteTransaction _transaction;
        public void BeginTransaction()
        {
            Open();
            _transaction = _conn.BeginTransaction();
            if (_dbLog) LogToFile("Started Transaction");
        }

        public void EndTransaction()
        {
            _transaction.Commit();
            if (_dbLog) LogToFile("Commited Transaction");
            _transaction = null;
            Close();
        }

        public static int DbInt(object dbObject)
        {
            return int.TryParse(dbObject.ToString(), out int i) ? i : -1;
        }

        public static double DbDouble(object dbObject)
        {
            return double.TryParse(dbObject.ToString(), out double i) ? i : -1;
        }

        public static DateTime DbDateTime(object dbObject)
        {
            return DateTime.TryParse(dbObject.ToString(), out DateTime upDateTime) ? upDateTime : DateTime.MinValue;
        }

        public static bool GetImageStatus(object imageNSFW)
        {
            if (!int.TryParse(imageNSFW.ToString(), out int i)) return false;
            return i == 1;
        }


        /// <summary>
        /// Convert list of objects to JSON array string.
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <returns>JSON array string</returns>
        private static string ListToJsonArray(ICollection<object> objects)
        {
            return JsonConvert.SerializeObject(objects);
        }

        /*
                /// <summary>
                /// Row describing table from sqlite_master
                /// </summary>
                private class TableRow
                {
                    private int Cid { get; set; }
                    private string Name { get; set; }
                    private string Type { get; set; }
                    private int Notnull { get; set; }
                    private string Dflt_value { get; set; }
                    private int Pk { get; set; }
                    private TableRow(int cid, string name, string type, int notnull, string dflt_value, int pk)
                    {
                        Cid = cid;
                        Name = name;
                        Type = type;
                        Notnull = notnull;
                        Dflt_value = dflt_value;
                        Pk = pk;
                    }
                }
        */

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Contains versions of the DB table.
        /// </summary>
        private enum DatabaseVersion
        {
            Pre = 0,
            V1_4_0 = 1,
            V1_4_7 = 2,
            Latest = 2
        }
        // ReSharper restore InconsistentNaming
        #endregion

    }

}