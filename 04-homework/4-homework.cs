using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

class Program 
{
    static void Main()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Products"].ConnectionString;

        Console.WriteLine("Choose what to use: 1 - SQL Server, 2 - MySQL, 3 - PostgreSQL");
        var dbChoice = Console.ReadLine();

        string providerName = dbChoice switch
        {
            "1" => "System.Data.SqlClient",
            "2" => "MySql.Data.MySqlClient",
            "3" => "Npgsql",
            _ => throw new Exception("Invalid choice. Please choose 1, 2 or 3.")
        };

        DbProviderFactories.RegisterFactory(providerName, SqlClientFactory.Instance);

        if (providerName == "System.Data.SqlClient") {
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Connection was successful\n");

                    var command = factory.CreateCommand();
                    command.Connection = connection;

                    command.CommandText = "SELECT * FROM Products";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            Console.WriteLine(string.Format(
                                "Name: {0,-10}\tType: {1,-10}\tColor: {2,-10}\tCalories: {3,-10}",
                                reader["name"], reader["type"], reader["color"], reader["calories"])
                            );
                    }

                    command.CommandText = "UPDATE Products SET name = 'Potato' WHERE id = 1";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "DELETE FROM Products WHERE id = 30";
                    await command.ExecuteNonQueryAsync();

                    await connection.CloseAsync();
                }
                catch (DbException ex)
                {
                    Console.WriteLine("Error while connecting to database: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine("\nConnection to database closed");
                }
            }
        }

        using (var connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            try
            {
                var stopwatch = Stopwatch.StartNew();

                connection.Open();
                Console.WriteLine("Connection was successful\n");

                var command = factory.CreateCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Products";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Console.WriteLine(string.Format(
                            "Name: {0,-10}\tType: {1,-10}\tColor: {2,-10}\tCalories: {3,-10}",
                            reader["name"], reader["type"], reader["color"], reader["calories"]));
                }

                Console.WriteLine($"Time of the request: {stopwatch.Elapsed.TotalSeconds} seconds\n");

                command.CommandText = "SELECT Name FROM Products";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Console.WriteLine($"Name: {reader["name"]}");
                }

                Console.WriteLine($"Time of the request: {stopwatch.Elapsed.TotalSeconds} seconds.\n");

                command.CommandText = "SELECT DISTINCT color FROM Products";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Console.WriteLine($"Color: {reader["color"]}");
                }

                Console.WriteLine($"Time of the request: {stopwatch.Elapsed.TotalSeconds} seconds\n");

                command.CommandText = "SELECT MAX(calories) FROM Products";
                Console.WriteLine($"Max calories: {command.ExecuteScalar()}");
                Console.WriteLine($"Time of the request: {stopwatch.Elapsed.TotalSeconds} seconds\n");

                command.CommandText = "SELECT MIN(calories) FROM Products";
                Console.WriteLine($"Min calories: {command.ExecuteScalar()}");
                Console.WriteLine($"Time of the request: {stopwatch.Elapsed.TotalSeconds} seconds\n");

                command.CommandText = "SELECT AVG(calories) FROM Products";
                Console.WriteLine($"Average calories: {command.ExecuteScalar()}");
                Console.WriteLine($"Time of the request: {stopwatch.Elapsed.TotalSeconds} seconds\n");
            }
            catch (DbException ex)
            {
                Console.WriteLine("Error while connecting to database: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("\nConnection to database closed");
            }
        }
    }
}