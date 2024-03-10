using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string connectionString = ConfigurationManager.ConnectionStrings["TeaShop"].ConnectionString;


        // 1
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM TeaShop", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "TeaShop");

            DataTable teaTable = dataSet.Tables["TeaShop"];
            DataRow newRow = teaTable.NewRow();
            newRow["name"] = "English Tea";
            newRow["description"] = "English Tea is a traditional blend of teas from Assam, Ceylon, and Kenya. It is a perfect tea for any time of the day.";
            newRow["id_country"] = 2;
            newRow["price"] = 4.59;
            newRow["weight"] = 300;
            newRow["id_type"] = 1;
            teaTable.Rows.Add(newRow);

            DataRow editRow = teaTable.Rows[5];
            editRow["name"] = "Matte";

            DataRow deleteRow = teaTable.Rows[10];
            teaTable.Rows.Remove(deleteRow);

            adapter.Update(dataSet, "TeaShop");
        }

        // 2
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM TeaShop", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "TeaShop");

            DataTable teaTable = dataSet.Tables["TeaShop"];

            var cherryTea = teaTable.AsEnumerable()
                .Where(r => r.Field<string>("description")
                .Contains("cherry"));

            Console.WriteLine("Tea which contains cherry in description:");
            foreach (var row in cherryTea)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name")}\n" +
                    $"Description: {row.Field<string>("description")}"
                );

            var priceRangeTea = teaTable.AsEnumerable()
                .Where(r => r.Field<decimal>("price") >= 10
                && r.Field<decimal>("price") <= 20);

            Console.WriteLine("\nTea with price in specified range:");
            foreach (var row in priceRangeTea)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var weightRangeTea = teaTable.AsEnumerable()
                .Where(r => r.Field<double>("weight") >= 200
                && r.Field<double>("weight") <= 500);

            Console.WriteLine("\nTea with weight in specified range:");
            foreach (var row in weightRangeTea)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"Weight: {row.Field<double>("weight"),-10}"
                );

            var specificCountryTea = teaTable.AsEnumerable()
                .Where(r => r.Field<int>("id_country") == 1
                || r.Field<int>("id_country") == 2);

            Console.WriteLine("\nTea from specified countries:");
            foreach (var row in specificCountryTea)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"ID of the country: {row.Field<int>("id_country"),-10}"
                );
        }

        // 3
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM TeaShop JOIN Country ON TeaShop.id_country = Country.id_country", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "TeaShop");

            DataTable teaTable = dataSet.Tables["TeaShop"];

            var teaByCountry = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() });

            Console.WriteLine("Country name and number of tea sorts:");
            foreach (var item in teaByCountry)
                Console.WriteLine(
                    $"Name: {item.Country,-10}" +
                    $"Amount of tea types: {item.Count,-10}"
                );

            var averageWeightByCountry = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, AverageWeight = g.Average(r => r.Field<double>("weight")) });

            Console.WriteLine("\nAverage weight of tea by country:");
            foreach (var item in averageWeightByCountry)
                Console.WriteLine(
                    $"Name: {item.Country,-10}" +
                    $"Average weight: {Math.Round(item.AverageWeight, 2),-10}"
                );

            var cheapestTeaByCountry = teaTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "India")
                .OrderBy(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree cheapest tea (India):");
            foreach (var row in cheapestTeaByCountry)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var mostExpensiveTeaByCountry = teaTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "India")
                .OrderByDescending(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree most expensive tea (India)");
            foreach (var row in mostExpensiveTeaByCountry)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var cheapestTea = teaTable.AsEnumerable()
                .OrderBy(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\nThree cheepest types of tea (all countries):");
            foreach (var row in cheapestTea)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );

            var mostExpensiveTea = teaTable.AsEnumerable()
                .OrderByDescending(r => r.Field<decimal>("price"))
                .Take(3);

            Console.WriteLine("\Three most expensive teas (all countries):");
            foreach (var row in mostExpensiveTea)
                Console.WriteLine(
                    $"Name: {row.Field<string>("name"),-20}" +
                    $"Price: {row.Field<decimal>("price"),-10}"
                );
        }

        // 4
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM TeaShop" +
                "JOIN Country ON TeaShop.id_country = Country.id_country " +
                "JOIN TeaType ON TeaShop.id_type = TeaType.id_type",
                connection
            );
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "TeaShop");

            DataTable teaTable = dataSet.Tables["TeaShop"];

            var top3CountriesByTeaTypes = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(3);

            Console.WriteLine("Top 3 countries by tea types:");
            foreach (var item in top3CountriesByTeaTypes)
                Console.WriteLine(
                    $"Country: {item.Country,-8}" +
                    $"Amount of tea types: {item.Count,-8}"
                );

            var top3CountriesByTeaWeight = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, TotalWeight = g.Sum(r => r.Field<double>("weight")) })
                .OrderByDescending(g => g.TotalWeight)
                .Take(3);

            Console.WriteLine("\n\nTop 3 countries by weight:");
            foreach (var item in top3CountriesByTeaWeight)
                Console.WriteLine(
                    $"Country: {item.Country,-8}" +
                    $"Overall tea weight: {item.TotalWeight,-8}"
                );

            Console.WriteLine();

            string[] teaTypes = { "Green tea", "Black tea" };
            foreach (var teaType in teaTypes)
            {
                var top3TeaByWeight = teaTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == teaType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nTop 3 tea types \"{teaType}\" by weight:");
                foreach (var row in top3TeaByWeight)
                    Console.WriteLine(
                        $"Name: {row.Field<string>("name"),-20}" +
                        $"Weight: {row.Field<double>("weight"),-10}"
                    );
            }
            Console.WriteLine();

            var allTeaTypes = teaTable.AsEnumerable()
                .Select(r => r.Field<string>("type_name"))
                .Distinct();

            foreach (var teaType in allTeaTypes)
            {
                var top3TeaByWeight = teaTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == teaType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nTop 3 tea types \"{teaType}\" by weight:");
                foreach (var row in top3TeaByWeight)
                    Console.WriteLine(
                        $"Name: {row.Field<string>("name"),-20}" +
                        $"Weight: {row.Field<double>("weight"),-10}"
                    );
            }
        }
    }
}