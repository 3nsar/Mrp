using Mrp.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mrp.Repositories
{
    public class UserRepository
    {
        private const string ConnectionString = "Host=localhost;Port=5431;Database=media-db;Username=postgres;Password=mysecretpassword";

        public bool CreateUser(string username, string passwordHash, string salt)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                string query = "INSERT INTO users (username, password_hash, salt) VALUES (@username, @hash, @salt)";
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("hash", passwordHash);
                command.Parameters.AddWithValue("salt", salt);
                command.ExecuteNonQuery();
                return true;
            }
            catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                return false;
            }
        }

        public UserModel? GetByUsername(string username)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT id, username, password_hash, salt FROM users WHERE username = @username";
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("username", username);
            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            return new UserModel
            {
                ID = reader.GetGuid(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Salt = reader.GetString(3)
            };
        }

        public void SaveToken(Guid userId, string token)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            string query = "UPDATE users SET token = @token WHERE id = @id";
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("token", token);
            command.Parameters.AddWithValue("id", userId);
            command.ExecuteNonQuery();
        }

        public bool TokenExists(string token)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            string query = "SELECT 1 FROM users WHERE token = @token";
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("token", token);
            return command.ExecuteScalar() != null;
        }
    }
}
