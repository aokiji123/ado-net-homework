using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=DESKTOP-BONEFOJ;SQLSERVER;Initial Catalog=OfficeSupplies;Integrated Security=True";

        // 1
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query1_1 = "INSERT INTO Products (name, type) VALUES (@name, @type)";
                using (SqlCommand command = new SqlCommand(query1_1, connection))
                {
                    command.Parameters.AddWithValue("@name", "Pen");
                    command.Parameters.AddWithValue("@type", "Office supplies");
                    command.ExecuteNonQuery();
                }

                string query1_2 = "INSERT INTO Managers (name, role) VALUES (@name, @role)";
                using (SqlCommand command = new SqlCommand(query1_2, connection))
                {
                    command.Parameters.AddWithValue("@name", "Alex");
                    command.Parameters.AddWithValue("@role", "HR manager");
                    command.ExecuteNonQuery();
                }

                string query1_3 = "INSERT INTO Companies (name) VALUES (@name)";
                using (SqlCommand command = new SqlCommand(query1_3, connection))
                {
                    command.Parameters.AddWithValue("@name", "");
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("\nError while connecting to database: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("\nConnection to database closed");
            }
        }

        // 2
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query2_1 = "UPDATE Products SET type = @type WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query2_1, connection))
                {
                    command.Parameters.AddWithValue("@name", "Pen");
                    command.Parameters.AddWithValue("@type", "Stationery supplies");
                    command.ExecuteNonQuery();
                }

                string query2_2 = "UPDATE Managers SET role = @role WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query2_2, connection))
                {
                    command.Parameters.AddWithValue("@name", "Dan");
                    command.Parameters.AddWithValue("@role", "HR manager");
                    command.ExecuteNonQuery();
                }

                string query2_3 = "UPDATE Companies SET name = @new_name WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query2_3, connection))
                {
                    command.Parameters.AddWithValue("@name", "Company");
                    command.Parameters.AddWithValue("@new_name", "Company LLC");
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("\nError while connecting to database: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("\nConnection to database closed");
            }
        }

        // 3
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query3_1 = "INSERT INTO ArchivedProducts SELECT * FROM Products WHERE name = @name "
                    + "DELETE FROM Products WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query3_1, connection))
                {
                    command.Parameters.AddWithValue("@name", "Pencil");
                    command.ExecuteNonQuery();
                }

                string query3_2 = "INSERT INTO ArchivedManagers SELECT * FROM Managers WHERE name = @name "
                    + "DELETE FROM Managers WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query3_2, connection))
                {
                    command.Parameters.AddWithValue("@name", "Bob");
                    command.ExecuteNonQuery();
                }

                string query3_3 = "INSERT INTO ArchivedCompanies SELECT * FROM Companies WHERE name = @name "
                    + "DELETE FROM Companies WHERE name = @name";
                using (SqlCommand command = new SqlCommand(query3_3, connection))
                {
                    command.Parameters.AddWithValue("@name", "Company LLC");
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("\nError while connecting to database: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("\nConnection to database closed");
            }
        }

        // 4
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query4_1 = "SELECT TOP (1) name FROM Managers ORDER BY units_sold DESC";
                using (SqlCommand command = new SqlCommand(query4_1, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["name"]);
                    }
                }

                string query4_2 = "SELECT TOP (1) name FROM Managers ORDER BY total_profit DESC";
                using (SqlCommand command = new SqlCommand(query4_2, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["name"]);
                    }
                }

                string query4_3 = "SELECT TOP (1) name FROM Managers WHERE last_sale_date BETWEEN DATEADD(year, -1, GETDATE()) AND GETDATE() ORDER BY total_profit DESC";
                using (SqlCommand command = new SqlCommand(query4_3, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["name"]);
                    }
                }

                string query4_4 = "SELECT TOP (1) name FROM Companies ORDER BY total_purchase DESC";
                using (SqlCommand command = new SqlCommand(query4_4, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["name"]);
                    }
                }

                string query4_5 = "SELECT TOP (1) type FROM Products ORDER BY units_sold DESC";
                using (SqlCommand command = new SqlCommand(query4_5, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["type"]);
                    }
                }

                string query4_6 = "SELECT TOP (1) type FROM Products ORDER BY total_profit DESC";
                using (SqlCommand command = new SqlCommand(query4_6, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["type"]);
                    }
                }

                string query4_7 = "SELECT TOP (1) name FROM Products ORDER BY units_sold DESC";
                using (SqlCommand command = new SqlCommand(query4_7, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["name"]);
                    }
                }

                string query4_8 = "SELECT name FROM Products WHERE DATEDIFF(day, last_sold_date, GETDATE()) > @days";
                int days = 30;
                using (SqlCommand command = new SqlCommand(query4_8, connection))
                {
                    command.Parameters.AddWithValue("@days", days);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(reader["name"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("\nError while connecting to database: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("\nConnection to database closed");
            }
        }
    }
}