using System.ComponentModel.Design;
using System.Data.SqlClient;
string CONNECTION_STRING = "Data Source=DESKTOP-BONEFOJ;Initial Catalog=Groceries;Integrated Security=True";

using(SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
    try {
        connection.Open();
        Console.WriteLine("Database connected successfully");
    } catch (SqlException ex) {
        Console.WriteLine("Error while connecting: " + ex.Message);
    } finally {
        try {
            connection.Close();
            Console.WriteLine("Database connection closed safely\n\n");
        } catch (SqlException ex) {
            Console.WriteLine("Error closing connection: " + ex.Message);
        }
    }
}

using(SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
    try {
        connection.Open();
        Console.WriteLine("Database connection established\n");

        SqlCommand cmd = new SqlCommand("SELECT * FROM Products", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read()) 
            Console.WriteLine($"Name: {reader["name"],-10}\tType: {reader["type"],-10}\tColor: {reader["color"],-10}\tCalories: {reader["calories"],-10}");
        reader.Close();

        Console.WriteLine();

        cmd = new SqlCommand("SELECT Name FROM Products", connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) 
            Console.WriteLine($"Product Name: {reader["name"]}");
        reader.Close();

        Console.WriteLine();

        cmd = new SqlCommand("SELECT DISTINCT color FROM Products", connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) Console.WriteLine($"Color: {reader["
            color "]}");
        reader.Close();

        Console.WriteLine();

        cmd = new SqlCommand("SELECT MAX(calories) FROM Products", connection);
        Console.WriteLine($"Highest Calorie Count: {cmd.ExecuteScalar()}\n");

        cmd = new SqlCommand("SELECT MIN(calories) FROM Products", connection);
        Console.WriteLine($"Lowest Calorie Count: {cmd.ExecuteScalar()}\n");

        cmd = new SqlCommand("SELECT AVG(calories) FROM Products", connection);
        Console.WriteLine($"Average Calorie Count: {cmd.ExecuteScalar()}\n");

        connection.Close();
        Console.WriteLine("Database connection terminated\n\n");
    } catch (SqlException ex) {
        Console.WriteLine("Database access error: " + ex.Message);
    }
}

using(SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
    try {
        connection.Open();
        Console.WriteLine("Database connection reopened\n");

        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Products WHERE type = 'Vegetable'", connection);
        Console.WriteLine($"Total Vegetables: {cmd.ExecuteScalar()} \n");

        cmd = new SqlCommand("SELECT COUNT(*) FROM Products WHERE type = 'Fruit'", connection);
        Console.WriteLine($"Total Fruits: {cmd.ExecuteScalar()} \n");

        string color = "Green";
        cmd = new SqlCommand($"SELECT COUNT(*) FROM Products WHERE color = '{color}'", connection);
        Console.WriteLine($"Number of Green Products: {cmd.ExecuteScalar()}\n");

        cmd = new SqlCommand("SELECT color, COUNT(*) FROM Products GROUP BY color", connection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read()) Console.WriteLine($"Color: {reader[0]}, Amount: {reader[1]}");
         reader.Close();

        Console.WriteLine();

        int maxCalories = 40;
        cmd = new SqlCommand($"SELECT * FROM Products WHERE calories < {maxCalories}", connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) 
            Console.WriteLine($"Name: {reader["name"],-10}\tType: {reader["type"],-10}\tColor: {reader["color"],-10}\tCalories: {reader["calories"],-10}");
        reader.Close();

        Console.WriteLine();

        int minCalories = 80;
        cmd = new SqlCommand($"SELECT * FROM Products WHERE calories > {minCalories}", connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) 
            Console.WriteLine($"Name: {reader["name"],-10}\tType: {reader["type"],-10}\tColor: {reader["color"],-10}\tCalories: {reader["calories"],-10}");
        reader.Close();

        Console.WriteLine();

        int mnCalories = 40;
        int mxCalories = 80;

        cmd = new SqlCommand($"SELECT * FROM Products WHERE calories BETWEEN {mnCalories} AND {mxCalories}", connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) 
            Console.WriteLine($"Name: {reader["name"],-10}\tType: {reader["type"],-10}\tColor: {reader["color"],-10}\tCalories: {reader["calories"],-10}");
        reader.Close();

        Console.WriteLine();

        cmd = new SqlCommand("SELECT * FROM Products WHERE color IN ('Yellow', 'Red')", connection);
        reader = cmd.ExecuteReader();

        while (reader.Read()) 
            Console.WriteLine($"Name: {reader["name"],-10}\tType: {reader["type"],-10}\tColor: {reader["color"],-10}\tCalories: {reader["calories"],-10}");
        reader.Close();

        Console.WriteLine();
    } catch (SqlException ex) {
        Console.WriteLine("Error during database operation: " + ex.Message);
    } finally {
        connection.Close();
        Console.WriteLine("Database connection closed");
    }
}
