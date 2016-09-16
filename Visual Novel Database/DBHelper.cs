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

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace Happy_Search
{
    internal class DbHelper
    {
        private const string DbFile = "VNDBPC-database.sqlite";

        private const string DbConnectionString = "Data Source=" + DbFile + ";Version=3;";
        //Pooling=True;Max Pool Size=100;

        private const bool PrintGetMethods = false;
        public static SQLiteConnection DbConn;

        public DbHelper()
        {
            DbConn = new SQLiteConnection(DbConnectionString);
            InitDatabase();
        }

        #region Set Methods
        
        public void AddRelationsToVN(int vnid, List<RelationsItem> relations)
        {
            var relationsString = relations.Any() ? ListToJsonArray(new List<object>(relations)) : "Empty";
            relationsString = Regex.Replace(relationsString, "'", "''");
            var insertString =
                $"UPDATE vnlist SET Relations = '{relationsString}' WHERE VNID = {vnid};";
            var command = new SQLiteCommand(insertString, DbConn);
            command.ExecuteNonQuery();
        }

        public void AddScreensToVN(int vnid, List<ScreenItem> screens)
        {
            var screensString = screens.Any() ? ListToJsonArray(new List<object>(screens)) : "Empty";
            var insertString =
                $"UPDATE vnlist SET Screens = '{screensString}' WHERE VNID = {vnid};";
            var command = new SQLiteCommand(insertString, DbConn);
            command.ExecuteNonQuery();
        }

        public void AddAnimeToVN(int vnid, List<AnimeItem> anime)
        {
            var animeString = anime.Any() ? ListToJsonArray(new List<object>(anime)) : "Empty";
            animeString = Regex.Replace(animeString, "'", "''");
            var insertString =
                $"UPDATE vnlist SET Anime = '{animeString}' WHERE VNID = {vnid};";
            var command = new SQLiteCommand(insertString, DbConn);
            command.ExecuteNonQuery();
        }

        public void UpdateVNToLatestVersion(VNItem vnItem)
        {
            var insertString =
                $"UPDATE vnlist SET Popularity = {vnItem.Popularity:0.00}, Rating = {vnItem.Rating:0.00}, VoteCount = {vnItem.VoteCount} WHERE VNID = {vnItem.ID};";
            var command = new SQLiteCommand(insertString, DbConn);
            command.ExecuteNonQuery();
        }

        public void UpdateVNTags(VNItem vnItem)
        {
            var tags = ListToJsonArray(new List<object>(vnItem.Tags));
            var insertString =
                $"UPDATE vnlist SET Tags = '{tags}' WHERE VNID = {vnItem.ID};";
            var command = new SQLiteCommand(insertString, DbConn);
            command.ExecuteNonQuery();
        }

        public void UpdateVNStatus(int userID, int vnid, ChangeType type, int statusInt, Command command)
        {
            if (command == Command.Delete)
            {
                string deleteString = $"DELETE FROM userlist WHERE VNID = {vnid} AND UserID = {userID};";
                LogToFile(deleteString);
                var cmd = new SQLiteCommand(deleteString, DbConn);
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
                    switch (command)
                    {
                        case Command.New:
                            commandString =
                                $"INSERT OR REPLACE  INTO userlist (VNID, UserID, Vote, VoteAdded) Values ({vnid}, {userID}, {statusInt*10}, {statusDate});";
                            break;
                        case Command.Update:
                            commandString =
                                $"UPDATE userlist SET Vote = {statusInt*10}, VoteAdded = {statusDate} WHERE VNID = {vnid} AND UserID = {userID};";
                            break;
                    }
                    break;
            }
            if (commandString.Equals("")) return;
            LogToFile(commandString);
            var cmd2 = new SQLiteCommand(commandString, DbConn);
            cmd2.ExecuteNonQuery();
        }

        public void InsertFavoriteProducers(List<ListedProducer> addProducerList, int userid)
        {
            foreach (var item in addProducerList)
            {
                var insertString =
                    $"INSERT OR REPLACE INTO userprodlist (ProducerID, UserID, UserAverageVote, UserDropRate) VALUES ({item.ID}, {userid}, {item.UserAverageVote}, {item.UserDropRate});";
                var command = new SQLiteCommand(insertString, DbConn);
                LogToFile(insertString);
                command.ExecuteNonQuery();
            }
        }

        public void InsertProducer(ListedProducer producer)
        {
            var name = Regex.Replace(producer.Name, "'", "''");
            var insertString =
                $"INSERT OR REPLACE INTO producerlist (ProducerID, Name, Titles, Loaded) VALUES ({producer.ID}, '{name}', {producer.NumberOfTitles}, '{producer.Loaded}');";
            LogToFile(insertString);
            var cmd = new SQLiteCommand(insertString, DbConn);
            cmd.ExecuteNonQuery();
        }

        public void UpsertUserList(int userid, UserListItem item, bool update)
        {
            if (item.Notes == null) item.Notes = "";
            var note = Regex.Replace(item.Notes, "'", "''");
            var commandString =
                $"UPDATE userlist SET ULStatus = '{item.Status}', ULAdded = '{item.Added}', ULNote = '{note}' WHERE VNID = {item.VN} AND UserID = {userid};";
            var insertString =
                $"INSERT INTO userlist (VNID, UserID, ULStatus, ULAdded, ULNote) VALUES ({item.VN},{userid},'{item.Status}', '{item.Added}', '{note}');";
            if (!update) commandString = insertString;
            var command = new SQLiteCommand(commandString, DbConn);
            LogToFile(commandString);
            command.ExecuteNonQuery();
        }

        public void UpsertWishList(int userid, WishListItem item, bool update)
        {
            var commandString =
                $"UPDATE userlist SET WLStatus = '{item.Priority}', WLAdded = '{item.Added}' WHERE VNID = '{item.VN}' AND UserID = '{userid}';";
            var insertString =
                $"INSERT INTO userlist (VNID, WLStatus, WLAdded, UserID) VALUES ('{item.VN}','{item.Priority}','{item.Added}', '{userid}');";
            if (!update) commandString = insertString;
            var command = new SQLiteCommand(commandString, DbConn);
            LogToFile(commandString);
            command.ExecuteNonQuery();
        }

        public void UpsertVoteList(int userid, VoteListItem item, bool update)
        {
            var commandString =
                $"UPDATE userlist SET Vote = '{item.Vote}', VoteAdded = '{item.Added}' WHERE VNID = '{item.VN}' AND UserID = '{userid}';";
            var insertString =
                $"INSERT INTO userlist (VNID, Vote, VoteAdded, UserID) VALUES ('{item.VN}','{item.Vote}','{item.Added}', '{userid}');";
            if (!update) commandString = insertString;
            var command = new SQLiteCommand(commandString, DbConn);
            LogToFile(commandString);
            command.ExecuteNonQuery();
        }


        public void UpsertSingleCharacter(CharacterItem character)
        {
            var insertString =
                $"INSERT OR REPLACE INTO charlist (CharacterID, Traits, VNs) VALUES ('{character.ID}', '{ListToJsonArray(new List<object>(character.Traits))}','{ListToJsonArray(new List<object>(character.VNs))}');";
            var command = new SQLiteCommand(insertString, DbConn);
            command.ExecuteNonQuery();
        }

        public void UpsertSingleVN(VNItem item, int producerid, bool print = true)
        {
            var tags = ListToJsonArray(new List<object>(item.Tags));
            var title = Regex.Replace(item.Title, "'", "''");
            var kanjiTitle = item.Original != null ? Regex.Replace(item.Original, "'", "''") : "";
            var description = item.Description != null ? Regex.Replace(item.Description, "'", "''") : "";
            int? length = item.Length ?? 0;
            var insertString =
                "INSERT OR REPLACE INTO vnlist (Title, KanjiTitle, ProducerID, RelDate, Tags, Description, ImageURL, ImageNSFW, LengthTime, Popularity, Rating, VoteCount, VNID)" +
                $"VALUES('{title}', '{kanjiTitle}', {producerid}, '{item.Released}', '{tags}', '{description}', '{item.Image}', {SetImageStatus(item.Image_Nsfw)}, " +
                $"{length}, {item.Popularity:0.00},{item.Rating:0.00}, {item.VoteCount}, {item.ID});";
            var command = new SQLiteCommand(insertString, DbConn);
            if (print) LogToFile(insertString);
            command.ExecuteNonQuery();
        }

        public void RemoveFavoriteProducer(int producerID, int userid)
        {
            var commandString = $"DELETE FROM userprodlist WHERE ProducerID={producerID} AND UserID={userid};";
            var command = new SQLiteCommand(commandString, DbConn);
            LogToFile(commandString);
            command.ExecuteNonQuery();
        }

        public void RemoveVisualNovel(int vnid)
        {
            var commandString = $"DELETE FROM vnlist WHERE VNID={vnid};";
            var command = new SQLiteCommand(commandString, DbConn);
            LogToFile(commandString);
            command.ExecuteNonQuery();
        }

        #endregion

        #region Get Methods

        public List<int> GetUnfetchedUserRelatedTitles(int userid)
        {
            var selectString =
                $"SELECT VNID FROM userlist WHERE VNID NOT IN (SELECT VNID FROM vnlist) AND UserID = {userid};";
            var list = new List<int>();
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(DbInt(reader["VNID"]));
            return list;
        }

        public ListedVN GetSingleVN(int vnid, int userid)
        {
            var selectString =
                $"SELECT vnlist.*, userlist.*, producerlist.Name FROM vnlist LEFT JOIN userlist ON vnlist.VNID = userlist.VNID AND userlist.UserID ={userid} LEFT JOIN producerlist ON producerlist.ProducerID = vnlist.ProducerID WHERE vnlist.VNID={vnid};";
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            ListedVN vn = null;
            while (reader.Read()) vn = GetListedVN(reader);
            return vn;
        }

        public string GetProducerName(int producerid)
        {
            var returnString = "N/A";
            var selectString = $"SELECT Name FROM producerlist WHERE ProducerID={producerid};";
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (
                reader.Read())
            {
                returnString = reader["Name"].ToString();
            }
            return returnString;
        }

        public List<ListedProducer> GetAllProducers()
        {
            var list = new List<ListedProducer>();
            var selectString = "SELECT * FROM producerlist;";
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(GetListedProducer(reader));
            }
            return list;
        }

        public List<int> GetProducerIDsForUser(int userid)
        {
            var producerIDList = new List<int>();
            var selectString = $"SELECT ProducerID FROM userprodlist WHERE UserID={userid};";
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) producerIDList.Add(DbInt(reader["ProducerID"]));
            return producerIDList;
        }

        public List<ListedProducer> GetFavoriteProducersForUser(int userid)
        {
            var readerList = new List<ListedProducer>();
            var selectString =
                $"SELECT producerlist.*, userprodlist.UserAverageVote, userprodlist.UserDropRate FROM producerlist LEFT JOIN userprodlist ON producerlist.ProducerID = userprodlist.ProducerID WHERE userprodlist.UserID = {userid};";
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(GetFavoriteProducer(reader));
            return readerList;
        }

        public List<int> GetUserRelatedIDs(int userid)
        {
            var readerList = new List<int>();
            var selectString = $"SELECT VNID FROM userlist WHERE UserID = {userid};";
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(DbInt(reader["VNID"]));
            return readerList;
        }

        public List<ListedVN> GetUserRelatedTitles(int userid)
        {
            var readerList = new List<ListedVN>();
            var selectString =
                $"SELECT vnlist.*, userlist.*, producerlist.Name FROM vnlist, userlist  LEFT JOIN producerlist ON producerlist.ProducerID = vnlist.ProducerID WHERE vnlist.VNID = userlist.VNID AND UserID = {userid};";
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(GetListedVN(reader));
            return readerList;
        }

        public List<ListedVN> GetAllTitles(int userid)
        {
            var readerList = new List<ListedVN>();
            var selectString =
                $"SELECT vnlist.*, userlist.*, producerlist.Name FROM vnlist LEFT JOIN userlist ON vnlist.VNID = userlist.VNID AND userlist.UserID ={userid} LEFT JOIN producerlist ON producerlist.ProducerID = vnlist.ProducerID WHERE Title NOT NULL;";
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(GetListedVN(reader));
            return readerList;
        }
        
        public List<CharacterItem> GetAllCharacters()
        {
            var readerList = new List<CharacterItem>();
            var selectString = "SELECT * FROM charlist;";
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
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
            if (PrintGetMethods) LogToFile(selectString);
            var command = new SQLiteCommand(selectString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) readerList.Add(GetListedVN(reader));
            return readerList;
        }

        public List<int> GetNotFetchedTitlesByProducerID(int producerid)
        {
            var idList = new List<int>();
            var selectIDsString = $"SELECT VNID FROM vnlist WHERE ProducerID={producerid} AND Title IS NULL;";
            var command = new SQLiteCommand(selectIDsString, DbConn);
            var reader = command.ExecuteReader();
            while (reader.Read()) idList.Add(DbInt(reader["VNID"]));
            return idList;
        }

        #endregion

        #region Other

        private CharacterItem GetCharacterItem(SQLiteDataReader reader)
        {
            return new CharacterItem
            {
                ID = DbInt(reader["CharacterID"]),
                Traits = JsonConvert.DeserializeObject<List<TraitItem>>(reader["Traits"].ToString()),
                VNs = JsonConvert.DeserializeObject<List<CharacterVNItem>>(reader["VNs"].ToString())
            };
        }

        private ListedVN GetListedVN(SQLiteDataReader reader)
        {
            return new ListedVN(
                reader["Title"].ToString(),
                reader["KanjiTitle"].ToString(),
                reader["RelDate"].ToString(),
                reader["Name"].ToString(),
                DbInt(reader["LengthTime"]),
                DbInt(reader["ULStatus"]),
                DbInt(reader["ULAdded"]),
                reader["ULNote"].ToString(),
                DbInt(reader["WLStatus"]),
                DbInt(reader["WLAdded"]),
                DbInt(reader["Vote"]),
                DbInt(reader["VoteAdded"]),
                reader["Tags"].ToString(),
                DbInt(reader["VNID"]),
                DbDateTime(reader["DateUpdated"]),
                reader["ImageURL"].ToString(),
                GetImageStatus(reader["ImageNSFW"]),
                reader["Description"].ToString(),
                DbDouble(reader["Popularity"]),
                DbDouble(reader["Rating"]),
                DbInt(reader["VoteCount"]),
                reader["Relations"].ToString(),
                reader["Screens"].ToString(),
                reader["Anime"].ToString());
        }

        private ListedProducer GetListedProducer(SQLiteDataReader reader)
        {
            return new ListedProducer(
                reader["Name"].ToString(),
                DbInt(reader["Titles"]),
                reader["Loaded"].ToString(),
                DbDateTime(reader["Updated"]),
                DbInt(reader["ProducerID"]));
        }

        private ListedProducer GetFavoriteProducer(SQLiteDataReader reader)
        {
            return new ListedProducer(
                reader["Name"].ToString(),
                DbInt(reader["Titles"]),
                reader["Loaded"].ToString(),
                DbDateTime(reader["Updated"]),
                DbInt(reader["ProducerID"]),
                DbDouble(reader["UserAverageVote"]),
                DbInt(reader["UserDropRate"]));
        }

        public void Open()
        {
            if (DbConn.State == ConnectionState.Closed)
            {
                DbConn.Open();
            }
            else LogToFile("Tried to open database, it was already opened.");
        }

        public void Close()
        {
            DbConn.Close();
        }

        public int DbInt(object dbObject)
        {
            int i;
            if (!int.TryParse(dbObject.ToString(), out i)) return -1;
            return i;
        }

        public double DbDouble(object dbObject)
        {
            double i;
            if (!double.TryParse(dbObject.ToString(), out i)) return -1;
            return i;
        }

        public DateTime DbDateTime(object dbObject)
        {
            DateTime upDateTime;
            return !DateTime.TryParse(dbObject.ToString(), out upDateTime) ? DateTime.MinValue : upDateTime;
        }

        private static void InitDatabase()
        {
            if (File.Exists(DbFile)) return;
            SQLiteConnection.CreateFile(DbFile);
            DbConn.Open();
            //must be in this order
            CreateProducerListTable();
            CreateVNListTable();
            CreateCharacterListTable();
            CreateUserlistTable();
            CreateUserProdListTable();
            CreateTriggers();
            DbConn.Close();
        }

        private static void CreateVNListTable()
        {
            const string createCommand = @"CREATE TABLE ""vnlist"" ( `VNID` INTEGER NOT NULL UNIQUE,
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
PRIMARY KEY(`VNID`),
FOREIGN KEY(`ProducerID`) REFERENCES `ProducerID` )";
            var command = new SQLiteCommand(createCommand, DbConn);
            command.ExecuteNonQuery();
        }

        private static void CreateProducerListTable()
        {
            const string createCommand = @"CREATE TABLE `producerlist` (
	`ProducerID`	INTEGER NOT NULL,
	`Name`	TEXT,
	`Titles`	INTEGER,
	`Loaded`	TEXT,
	`Updated`	DATE DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(ProducerID)
)";
            var command = new SQLiteCommand(createCommand, DbConn);
            command.ExecuteNonQuery();
        }

        private static void CreateCharacterListTable()
        {
            var createCommand = @"CREATE TABLE ""charlist"" ( 
`CharacterID` INTEGER NOT NULL UNIQUE, 
`Name` TEXT, 
`Image` TEXT, 
`Traits` TEXT, 
`VNs` TEXT, 
`DateUpdated` INTEGER, 
PRIMARY KEY(`CharacterID`) )";
            var command = new SQLiteCommand(createCommand, DbConn);
            command.ExecuteNonQuery();
        }

        private static void CreateUserlistTable()
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
            var command = new SQLiteCommand(createCommand, DbConn);
            command.ExecuteNonQuery();
        }

        private static void CreateUserProdListTable()
        {
            const string createCommand = @"CREATE TABLE `userprodlist`(
	`ProducerID`	INTEGER NOT NULL,
	`UserID`	INTEGER NOT NULL,
	`UserAverageVote`	NUMERIC,
	`UserDropRate`	INTEGER,
	PRIMARY KEY(ProducerID, UserID)
)";
            var command = new SQLiteCommand(createCommand, DbConn);
            command.ExecuteNonQuery();
        }

        private static void CreateTriggers()
        {
            const string createCommand = @"CREATE TRIGGER [UpdateTimestamp]
    AFTER UPDATE    ON vnlist    FOR EACH ROW
BEGIN
    UPDATE vnlist 
	SET DateUpdated=CURRENT_TIMESTAMP
	WHERE VNID=OLD.VNID;
END";
            const string createCommand2 = @"CREATE TRIGGER [UpdateTimestampProducerList]
    AFTER UPDATE    ON producerlist    FOR EACH ROW
BEGIN
    UPDATE producerlist 
	SET Updated=CURRENT_TIMESTAMP
	WHERE ProducerID=OLD.ProducerID;
END";
            const string createCommand3 = @"CREATE TRIGGER [UpdateTimestampCharacterList] 
AFTER UPDATE ON charlist FOR EACH ROW 
BEGIN 
UPDATE charlist SET DateUpdated=CURRENT_TIMESTAMP 
WHERE CharacterID=OLD.CharacterID; 
END";
            var command = new SQLiteCommand(createCommand, DbConn);
            command.ExecuteNonQuery();
            var command2 = new SQLiteCommand(createCommand2, DbConn);
            command2.ExecuteNonQuery();
            var command3 = new SQLiteCommand(createCommand3, DbConn);
            command3.ExecuteNonQuery();
        }

        private static int SetImageStatus(bool imageNSFW)
        {
            var i = imageNSFW ? 1 : 0;
            return i;
        }

        private static bool GetImageStatus(object imageNSFW)
        {
            int i;
            if (!int.TryParse(imageNSFW.ToString(), out i)) return false;
            return i == 1;
        }

        
        /// <summary>
        /// Convert list of objects to JSON array string.
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <returns>JSON array string</returns>
        private static string ListToJsonArray(List<object> objects)
        {
            return JsonConvert.SerializeObject(objects);
        }

        #endregion
    }
}