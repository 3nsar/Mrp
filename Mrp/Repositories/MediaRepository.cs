using Mrp.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mrp.Repositories
{
    public class MediaRepository
    {
        public List<MediaModel> GetAllMedia()
        {
            List<MediaModel> mediaList = new List<MediaModel>();
            string connectionString = "Host=localhost;Port=5432;Database=ensar;Username=postgres;Password=ensar";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM media";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MediaModel media = new MediaModel
                            {
                                ID = reader.GetGuid(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                MediaType = reader.GetString(3),
                                ReleaseYear = reader.GetInt32(4),
                                Genre = reader.GetString(5),
                                AgeRestriction = reader.GetInt32(6),
                                AverageScore = reader.GetDouble(7),
                                FavoriteCount = reader.GetInt32(8)

                            };
                            mediaList.Add(media);
                        }
                    }
                }
            }
            return mediaList;
        }
    }
}
