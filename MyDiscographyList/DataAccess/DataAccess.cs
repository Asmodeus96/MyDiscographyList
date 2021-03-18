using Microsoft.Data.Sqlite;
using MyDiscographyList.Enum;
using MyDiscographyList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        static readonly string dbpath = System.Environment.CurrentDirectory + "\\DiscoDB.db";

        public static List<ArtistStatusModel> GetStatusList()
        {
            List<ArtistStatusModel> statusList = new List<ArtistStatusModel>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM Status";
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    statusList.Add(new ArtistStatusModel() { StatusId = query.GetInt32(0), StatusLabel = query.GetString(1), StatusColor = (Brush)new BrushConverter().ConvertFromString(query.GetString(2)) });
                }

                db.Close();
            }

            return statusList;
        }

        public static List<ArtistScoreModel> GetScoreList()
        {
            List<ArtistScoreModel> statusList = new List<ArtistScoreModel>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand("SELECT * FROM Score", db);
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM Score";
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    statusList.Add(new ArtistScoreModel() { ScoreId = query.GetInt32(0), ScoreLabel = query.GetString(1) });
                }

                db.Close();
            }

            return statusList;
        }

        public static int AddArtist(ArtistModel artist)
        {
            int insertedId = 0;

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO Artist (name, alias, scoreId, statusId, upToDate, date) VALUES (@Name, @Alias, @scoreId, @StatusId, @UpToDate, @Date); SELECT last_insert_rowid()";
                insertCommand.Parameters.AddWithValue("@Name", artist.ArtistName);
                insertCommand.Parameters.AddWithValue("@Alias", artist.ArtistAlias);
                insertCommand.Parameters.AddWithValue("@scoreId", artist.ArtistScore.ScoreId);
                insertCommand.Parameters.AddWithValue("@StatusId", artist.ArtistStatus.StatusId);
                insertCommand.Parameters.AddWithValue("@UpToDate", artist.ArtistUpToDate);
                insertCommand.Parameters.AddWithValue("@Date", DateTime.Now);

                SqliteDataReader query = insertCommand.ExecuteReader();

                while (query.Read())
                {
                    insertedId = query.GetInt32(0);
                }

                db.Close();
            }

            return insertedId;
        }

        public static ObservableCollection<ArtistModel> GetArtistList(TypeListEnum type, string filter)
        {
            ObservableCollection<ArtistModel> artistList = new ObservableCollection<ArtistModel>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                selectCommand.CommandText = "SELECT a.id, a.name, a.alias, a.upToDate, a.date, st.id, st.label, st.color, sc.id, sc.label FROM Artist a LEFT JOIN Status st ON st.id = a.statusId LEFT JOIN Score sc ON sc.id = a.scoreId ";

                switch (type)
                {
                    case TypeListEnum.Listened:
                        selectCommand.CommandText += "WHERE a.statusId <> 6 ";
                        break;

                    case TypeListEnum.Unlistened:
                        selectCommand.CommandText += "WHERE a.statusId = 6 ";
                        break;

                    case TypeListEnum.Searched:
                        selectCommand.CommandText += "WHERE 0 = 0 ";
                        break;

                }

                if (!string.IsNullOrEmpty(filter) && filter != "All")
                {
                    if (filter == "0-9")
                    {
                        selectCommand.CommandText += "AND a.name GLOB '[0-9]*' ";
                    }
                    else if (filter == "*")
                    {
                        selectCommand.CommandText += "AND a.name NOT GLOB '[0-9]*' AND a.name NOT GLOB '[A-Z]*' ";
                    }
                    else
                    {
                        selectCommand.CommandText += "AND a.name LIKE '" + filter + "%' ";
                    }
                }

                switch (type)
                {
                    case TypeListEnum.Listened:
                        selectCommand.CommandText += "ORDER BY a.name ";
                        break;

                    case TypeListEnum.Unlistened:
                        selectCommand.CommandText += "ORDER BY a.date ";
                        break;
                }

                selectCommand.CommandText += " ";

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    artistList.Add(new ArtistModel()
                    {
                        ArtistId = query.GetInt32(0),
                        ArtistName = query.GetString(1),
                        ArtistAlias = query.GetString(2),
                        ArtistUpToDate = query.GetBoolean(3),
                        ArtistDate = query.GetDateTime(4),
                        ArtistStatus = new ArtistStatusModel() { StatusId = query.GetInt32(5), StatusLabel = query.GetString(6), StatusColor = (Brush)new BrushConverter().ConvertFromString(query.GetString(7)) },
                        ArtistScore = new ArtistScoreModel() { ScoreId = query.GetInt32(8), ScoreLabel = query.GetString(9) }
                    });
                }

                db.Close();
            }

            return artistList;
        }

        public static void UpdateArtistScore(ArtistModel artist)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE Artist SET scoreId = @scoreId WHERE id = @Id ";
                updateCommand.Parameters.AddWithValue("@Id", artist.ArtistId);
                updateCommand.Parameters.AddWithValue("@scoreId", artist.ArtistScore.ScoreId);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void UpdateArtistStatus(ArtistModel artist)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE Artist SET statusId = @statusId, date = @Date WHERE id = @Id ";
                updateCommand.Parameters.AddWithValue("@Id", artist.ArtistId);
                updateCommand.Parameters.AddWithValue("@statusId", artist.ArtistStatus.StatusId);
                updateCommand.Parameters.AddWithValue("@Date", DateTime.Now);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void UpdateArtistUpToDate(ArtistModel artist)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE Artist SET upToDate = @UpToDate WHERE id = @Id ";
                updateCommand.Parameters.AddWithValue("@Id", artist.ArtistId);
                updateCommand.Parameters.AddWithValue("@UpToDate", artist.ArtistUpToDate);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void UpdateArtistName(ArtistModel artist)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE Artist SET name = @Name WHERE id = @Id ";
                updateCommand.Parameters.AddWithValue("@Id", artist.ArtistId);
                updateCommand.Parameters.AddWithValue("@Name", artist.ArtistName);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void UpdateArtistAlias(ArtistModel artist)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                updateCommand.CommandText = "UPDATE Artist SET alias = @Alias WHERE id = @Id ";
                updateCommand.Parameters.AddWithValue("@Id", artist.ArtistId);
                updateCommand.Parameters.AddWithValue("@Alias", artist.ArtistAlias);

                updateCommand.ExecuteReader();

                db.Close();
            }
        }

        public static ArtistModel GetArtistById(int id)
        {
            ArtistModel artist;

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                selectCommand.CommandText = "SELECT a.id, a.name, a.alias, a.upToDate, a.date, st.id, st.label, st.color, sc.id, sc.label FROM Artist a LEFT JOIN Status st ON st.id = a.statusId LEFT JOIN Score sc ON sc.id = a.scoreId WHERE a.id = @Id ";
                selectCommand.Parameters.AddWithValue("@Id", id);

                SqliteDataReader query = selectCommand.ExecuteReader();

                query.Read();

                artist = new ArtistModel()
                {
                    ArtistId = query.GetInt32(0),
                    ArtistName = query.GetString(1),
                    ArtistAlias = query.GetString(2),
                    ArtistUpToDate = query.GetBoolean(3),
                    ArtistDate = query.GetDateTime(4),
                    ArtistStatus = new ArtistStatusModel() { StatusId = query.GetInt32(5), StatusLabel = query.GetString(6), StatusColor = (Brush)new BrushConverter().ConvertFromString(query.GetString(7)) },
                    ArtistScore = new ArtistScoreModel() { ScoreId = query.GetInt32(8), ScoreLabel = query.GetString(9) }
                };

                db.Close();
            }

            return artist;
        }

        public static void AddRecommandation(int artistId1, int artistId2)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;


                insertCommand.CommandText = "INSERT INTO Recommandation (artistId1, artistId2) VALUES (@Id1, @Id2)";
                insertCommand.Parameters.AddWithValue("@Id1", artistId1);
                insertCommand.Parameters.AddWithValue("@Id2", artistId2);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static ObservableCollection<ArtistModel> GetRelatedArtist(int id)
        {
            ObservableCollection<ArtistModel> artistList = new ObservableCollection<ArtistModel>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                selectCommand.CommandText = @"SELECT * FROM ( 
                                                        SELECT a.id, a.name, a.alias, a.upToDate, a.date, st.id, st.label, st.color, sc.id, sc.label FROM Artist a 
                                                        LEFT JOIN Status st ON st.id = a.statusId 
                                                        LEFT JOIN Score sc ON sc.id = a.scoreId
                                                        INNER JOIN Recommandation r ON r.artistId1 = @Id
                                                        WHERE a.id <> @Id
                                                        UNION
                                                        SELECT a.id, a.name, a.alias, a.upToDate, a.date, st.id, st.label, st.color, sc.id, sc.label FROM Artist a 
                                                        LEFT JOIN Status st ON st.id = a.statusId 
                                                        LEFT JOIN Score sc ON sc.id = a.scoreId
                                                        INNER JOIN Recommandation r ON r.artistId2 = @Id
                                                        WHERE a.id <> @Id
                                                    )";

                selectCommand.Parameters.AddWithValue("@Id", id);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    artistList.Add(new ArtistModel()
                    {
                        ArtistId = query.GetInt32(0),
                        ArtistName = query.GetString(1),
                        ArtistAlias = query.GetString(2),
                        ArtistUpToDate = query.GetBoolean(3),
                        ArtistDate = query.GetDateTime(4),
                        ArtistStatus = new ArtistStatusModel() { StatusId = query.GetInt32(5), StatusLabel = query.GetString(6), StatusColor = (Brush)new BrushConverter().ConvertFromString(query.GetString(7)) },
                        ArtistScore = new ArtistScoreModel() { ScoreId = query.GetInt32(8), ScoreLabel = query.GetString(9) }
                    });
                }

                db.Close();
            }

            return artistList;
        }

        public static List<ArtistModel> CheckRelatedArtists(List<string> nameList)
        {
            List<ArtistModel> artistList = new List<ArtistModel>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;

                selectCommand.CommandText = @"SELECT a.id, a.name, a.alias, a.upToDate, a.date, st.id, st.label, st.color, sc.id, sc.label FROM Artist a LEFT JOIN Status st ON st.id = a.statusId LEFT JOIN Score sc ON sc.id = a.scoreId" +
                    " WHERE a.name IN (\""
                    + string.Join("\",\"", nameList) + "\") " +
                    " OR a.alias IN (\"" + string.Join("\",\"", nameList) + "\") ORDER BY a.name ";

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    artistList.Add(new ArtistModel()
                    {
                        ArtistId = query.GetInt32(0),
                        ArtistName = query.GetString(1),
                        ArtistAlias = query.GetString(2),
                        ArtistUpToDate = query.GetBoolean(3),
                        ArtistDate = query.GetDateTime(4),
                        ArtistStatus = new ArtistStatusModel() { StatusId = query.GetInt32(5), StatusLabel = query.GetString(6), StatusColor = (Brush)new BrushConverter().ConvertFromString(query.GetString(7)) },
                        ArtistScore = new ArtistScoreModel() { ScoreId = query.GetInt32(8), ScoreLabel = query.GetString(9) }
                    });
                }

                db.Close();
            }

            return artistList;
        }

        public static void DeleteArtist(ArtistModel artist)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = db;

                deleteCommand.CommandText = "DELETE FROM Recommandation WHERE artistId1 = @Id OR artistId2 = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", artist.ArtistId);

                deleteCommand.ExecuteReader();
                db.Close();

                db.Open();
                deleteCommand = new SqliteCommand();
                deleteCommand.Connection = db;
                deleteCommand.CommandText = "DELETE FROM Artist WHERE id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", artist.ArtistId);

                deleteCommand.ExecuteReader();

                db.Close();
            }
        }

        public static List<ArtistStatusModel> CountStatusDisco()
        {
            List<ArtistStatusModel> statusList = new List<ArtistStatusModel>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT s.id, s.label, s.color, COUNT(a.id) FROM Status s LEFT JOIN Artist a ON a.statusId = s.id GROUP BY s.id, s.label, s.color";
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    statusList.Add(new ArtistStatusModel() { StatusId = query.GetInt32(0), 
                        StatusLabel = query.GetString(1), 
                        StatusColor = (Brush)new BrushConverter().ConvertFromString(query.GetString(2)),
                        NbOfStatus = query.GetInt32(3)
                    });
                }

                db.Close();
            }

            return statusList;
        }
    }
}
