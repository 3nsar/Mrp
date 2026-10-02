using Npgsql;

string connectionString = 
using var connection = new NpgsqlConnection(connectionString);

try
{
    connection.Open();
    Console.WriteLine("Connection opened successfully.");
    Console.WriteLine($"Datenbank: {connection.Database}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while opening the connection: {ex.Message}");
}

