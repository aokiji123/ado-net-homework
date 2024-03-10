using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string connectionString = ConfigurationManager.ConnectionStrings["CoffeeShop"].ConnectionString;

        // 1
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CoffeeShop", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CoffeeShop");

            DataTable coffeeTable = dataSet.Tables["CoffeeShop"];
            DataRow newRow = coffeeTable.NewRow();
            newRow["name"] = "Costa Rica Reserva";
            newRow["description"] = "Coffee from Costa Rica";
            newRow["id_country"] = 1;
            newRow["price"] = 7.99;
            newRow["weight"] = 400;
            newRow["id_type"] = 3;
            coffeeTable.Rows.Add(newRow);

            DataRow editRow = coffeeTable.Rows[0];
            editRow["name"] = "Argentina Gold";

            DataRow deleteRow = coffeeTable.Rows[7];
            coffeeTable.Rows.Remove(deleteRow);

            adapter.Update(dataSet, "CoffeeShop");
        }

        // 2
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CoffeeShop", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CoffeeShop");

            DataTable coffeeTable = dataSet.Tables["CoffeeShop"];

            var cherryCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<string>("description")
                .Contains("cherry"));

            Console.WriteLine("Coffee which description contains 'cherry':");
            foreach (var row in cherryCoffee)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name")}\n" +
                    $"Description: {row.Field<string>("description")}"
                );

            var priceRangeCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<decimal>("price") >= 10
                && r.Field<decimal>("price") <= 20);

            Console.WriteLine("\nCoffee with price in specified range:");
            foreach (var row in priceRangeCoffee)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var weightRangeCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<double>("weight") >= 200 
                && r.Field<double>("weight") <= 500);

            Console.WriteLine("\nCoffee with weight in specified range:");
            foreach (var row in weightRangeCoffee)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"Weight: {row.Field<double>("weight"),-10}"
                );

            var specificCountryCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<int>("id_country") == 1 
                || r.Field<int>("id_country") == 2);

            Console.WriteLine("\nCoffee from specified countries:");
            foreach (var row in specificCountryCoffee)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"ID of the country: {row.Field<int>("id_country"),-10}"
                );
        }

        // 3
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CoffeeShop JOIN Country ON Coffee.id_country = Country.id_country", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CoffeeShop");

            DataTable coffeeTable = dataSet.Tables["CoffeeShop"];

            var coffeeByCountry = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() });

            Console.WriteLine("Name of the country and the number of coffee types:");
            foreach (var item in coffeeByCountry)
                Console.WriteLine(
                    $"Country: {item.Country,-10}" +
                    $"Amount of coffee types: {item.Count,-10}"
                );

            var averageWeightByCountry = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, AverageWeight = g.Average(r => r.Field<double>("weight")) });

            Console.WriteLine("\nAverage of the coffee weight by country:");
            foreach (var item in averageWeightByCountry)
                Console.WriteLine(
                    $"Country: {item.Country,-10}" +
                    $"Average weight: {Math.Round(item.AverageWeight,2),-10}"
                );

            var cheapestCoffeeByCountry = coffeeTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "Argentina")
                .OrderBy(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree cheapest coffee types (Argentina):");
            foreach (var row in cheapestCoffeeByCountry)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var mostExpensiveCoffeeByCountry = coffeeTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "Argentina")
                .OrderByDescending(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree most expensive coffee types (Argentina):");
            foreach (var row in mostExpensiveCoffeeByCountry)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var cheapestCoffee = coffeeTable.AsEnumerable()
                .OrderBy(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree cheapest coffee types (all countries):");
            foreach (var row in cheapestCoffee)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var mostExpensiveCoffee = coffeeTable.AsEnumerable()
                .OrderByDescending(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree most expensive coffee types (all countries):");
            foreach (var row in mostExpensiveCoffee)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-28}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );
        }

        // 4
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM CoffeeShop" +
                "JOIN Country ON Coffee.id_country = Country.id_country " +
                "JOIN CoffeeType ON Coffee.id_type = CoffeeType.id_type",
                connection
            );
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
           
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CoffeeShop");

            // Task 4
            DataTable coffeeTable = dataSet.Tables["CoffeeShop"];

            var top3CountriesByCoffeeVariety = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(3);

            Console.WriteLine("Top 3 countries by coffee variety:");
            foreach (var item in top3CountriesByCoffeeVariety)
                Console.WriteLine(
                    $"Country: {item.Country,-10}" +
                    $"Amount of coffee types: {item.Count,-10}"
                );

            var top3CountriesByCoffeeWeight = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, TotalWeight = g.Sum(r => r.Field<double>("weight")) })
                .OrderByDescending(g => g.TotalWeight)
                .Take(3);

            Console.WriteLine("\nTop 3 countries by coffee weight:");
            foreach (var item in top3CountriesByCoffeeWeight)
                Console.WriteLine(
                    $"Country: {item.Country,-10}" +
                    $"Overall coffee weight: {item.TotalWeight,-10}"
                );

            Console.WriteLine();

            string[] coffeeTypes = { "Arabica", "Strong", "Bland" };
            foreach (var coffeeType in coffeeTypes)
            {
                var top3CoffeeByWeight = coffeeTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == coffeeType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nTop 3 coffee types \"{coffeeType}\" by weight:");
                foreach (var row in top3CoffeeByWeight)
                    Console.WriteLine(
                        $"Name: {row.Field<string>("name"),-28}" +
                        $"Weight: {row.Field<double>("weight"),-10}"
                    );
            }
            Console.WriteLine();

            var allCoffeeTypes = coffeeTable.AsEnumerable()
                .Select(r => r.Field<string>("type_name"))
                .Distinct();

            foreach (var coffeeType in allCoffeeTypes)
            {
                var top3CoffeeByWeight = coffeeTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == coffeeType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nTop 3 coffee types \"{coffeeType}\" by weight:");
                foreach (var row in top3CoffeeByWeight)
                    Console.WriteLine(
                        $"Name: {row.Field<string>("name"),-28}" + 
                        $"Weight: {row.Field<double>("weight"),-10}"
                    );
            }
        }
    }
}