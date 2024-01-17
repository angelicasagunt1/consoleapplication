namespace Nobel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            string endpoint = "https://api.sampleapis.com/codingresources/codingResources";
            string jsonData = await GetJsonData(endpoint);

            if (!string.IsNullOrEmpty(jsonData))
            {
                List<string> topics = ExtractTopics(jsonData);
                PrintUniqueTopics(topics);
            }
        }

        static async Task<string> GetJsonData(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP Request Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return string.Empty;
        }

        static List<string> ExtractTopics(string jsonData)
        {
            List<string> topics = new List<string>();
            try
            {
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);

                foreach (JsonElement item in jsonDocument.RootElement.EnumerateArray())
                {
                    if (item.TryGetProperty("topics", out JsonElement topicsArray))
                    {
                        foreach (JsonElement topic in topicsArray.EnumerateArray())
                        {
                            topics.Add(topic.ToString());
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine("JSON Parsing Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return topics.Distinct().ToList();
        }

        static void PrintUniqueTopics(List<string> topics)
        {
            foreach (var topic in topics)
            {
                Console.WriteLine(topic);
            }
        }
    }
}
