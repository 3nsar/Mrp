using Npgsql;

//create env file 
string connectionString = 
using var connection = new NpgsqlConnection(connectionString);

try
{
    connection.Open();
    string query = "SELECT * FROM test";
    using var command = new NpgsqlCommand(query, connection);
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"name: {reader["name"]}, age: {reader["age"]}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while opening the connection: {ex.Message}");
}

