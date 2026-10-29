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
      }
    }
