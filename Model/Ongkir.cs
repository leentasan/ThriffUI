using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;

public static class Ongkir
{
    private const string API_KEY = "c31a7ac4eaed9d6d966f5af4cf2aa4b9";

    // Mendapatkan daftar kota/kabupaten
    public static Dictionary<string, string> GetCityMappings()
    {
        var client = new RestClient("https://api.rajaongkir.com/starter/city");
        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("key", "c31a7ac4eaed9d6d966f5af4cf2aa4b9");

        var response = client.Execute(request);

        if (response.IsSuccessful)
        {
            Console.WriteLine($"City Mappings Response: {response.Content}");
            dynamic data = JsonConvert.DeserializeObject(response.Content);

            var cityMappings = new Dictionary<string, string>();
            foreach (var city in data.rajaongkir.results)
            {
                cityMappings[city.city_name.ToString()] = city.city_id.ToString();
            }

            return cityMappings;
        }
        else
        {
            Console.WriteLine($"Error fetching city mappings: {response.ErrorMessage}");
            throw new Exception($"Error fetching city mappings: {response.ErrorMessage}");
        }
    }

    public static string GetCityIdByName(string cityName)
    {
        var cityMappings = GetCityMappings();

        if (cityMappings.ContainsKey(cityName))
        {
            return cityMappings[cityName];
        }
        else
        {
            throw new Exception($"City '{cityName}' not found in RajaOngkir mappings.");
        }
    }


    // Mendapatkan layanan pengiriman
    public static List<ShippingOption> GetShippingOptions(string originCityId, string destinationCityId, int weight, string courier)
    {
        var client = new RestClient("https://api.rajaongkir.com/starter/cost");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("key", API_KEY);
        request.AddHeader("content-type", "application/x-www-form-urlencoded");

        request.AddParameter("origin", originCityId);
        request.AddParameter("destination", destinationCityId);
        request.AddParameter("weight", weight);
        request.AddParameter("courier", courier);

        var response = client.Execute(request);

        if (response.IsSuccessful)
        {
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            var shippingOptions = new List<ShippingOption>();

            foreach (var service in data.rajaongkir.results[0].costs)
            {
                var option = new ShippingOption
                {
                    Text = $"{service.service} - Rp{service.cost[0].value} (ETD: {service.cost[0].etd} days)",
                    Value = service.service.ToString()
                };
                shippingOptions.Add(option);
            }

            return shippingOptions;
        }
        else
        {
            throw new Exception($"Error fetching shipping options: {response.ErrorMessage}");
        }
    }


    public static string GetSellerAddress(int productId)
    {
        // Dummy: Koneksi database untuk mendapatkan seller_id
        using (var conn = new Npgsql.NpgsqlConnection(""))
        {
            conn.Open();

            string query = "SELECT s.address FROM seller s JOIN product p ON s.sellerid = p.sellerid WHERE p.productid = @ProductId";
            using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("ProductId", productId);

                var result = cmd.ExecuteScalar();
                return result?.ToString() ?? "Unknown Address";
            }
        }
    }


    public class ShippingOption
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

}
