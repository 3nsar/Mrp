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

        public List<MediaModel> GetMediaById(Guid id)
        {
            List<MediaModel> mediaList = new List<MediaModel>();
            string connectionString = "Host=localhost;Port=5432;Database=ensar;Username=postgres;Password=ensar";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM media WHERE id = @id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
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

        public void AddMedia(MediaModel media)
        {
            string connectionString = "Host=localhost;Port=5432;Database=ensar;Username=postgres;Password=ensar";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO media ( title, description, media_type, release_year, genre, age_restriction) " +
                    "VALUES ( @title, @description, @media_type, @release_year, @genre, @age_restriction)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    //command.Parameters.AddWithValue("id", media.ID);
                    command.Parameters.AddWithValue("title", media.Title);
                    command.Parameters.AddWithValue("description", media.Description);
                    command.Parameters.AddWithValue("media_type", media.MediaType);
                    command.Parameters.AddWithValue("release_year", media.ReleaseYear);
                    command.Parameters.AddWithValue("genre", media.Genre);
                    command.Parameters.AddWithValue("age_restriction", media.AgeRestriction);
                    //command.Parameters.AddWithValue("average_score", media.AverageScore);
                    // command.Parameters.AddWithValue("favorite_count", media.FavoriteCount);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMediaById(Guid id, MediaModel media)
        {
            string connectionString = "Host=localhost;Port=5432;Database=ensar;Username=postgres;Password=ensar";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE media SET title = @title, description = @description, " +
                    "media_type = @media_type, release_year = @release_year, genre = @genre, age_restriction = @age_restriction WHERE id = @id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("title", media.Title);
                    command.Parameters.AddWithValue("description", media.Description);
                    command.Parameters.AddWithValue("media_type", media.MediaType);
                    command.Parameters.AddWithValue("release_year", media.ReleaseYear);
                    command.Parameters.AddWithValue("genre", media.Genre);
                    command.Parameters.AddWithValue("age_restriction", media.AgeRestriction);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMediaById(Guid id)
        {
            string connectionString = "Host=localhost;Port=5432;Database=ensar;Username=postgres;Password=ensar";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM media WHERE id = @id";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
