using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FindAppInsights;

class SubscriptionList
{
    [JsonPropertyName("value")]
    public List<Subscription> Subscriptions { get; set; }
}

class Subscription
{
    [JsonPropertyName("subscriptionId")]
    public string SubscriptionId { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
}

class AppInsightsList
{
    [JsonPropertyName("value")]
    public List<AppInsights> AppInsights { get; set; }
}

class AppInsights
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("properties")]
    public AppInsightsProperties Properties { get; set; }
}

class AppInsightsProperties
{
    [JsonPropertyName("InstrumentationKey")]
    public string InstrumentationKey { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        string token = "xxxxxxxxxxxxxxxxxxxx";
        Console.Write("Enter instrumentation key - ");
        string instrumentationKey = Console.ReadLine();
        HttpClient client = new HttpClient();
        Uri baseUri = new Uri(@"https://management.azure.com/subscriptions?api-version=2020-01-01");
        client.BaseAddress = baseUri;
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(baseUri);
        var data = await response.Content.ReadAsStringAsync();
        var listOfSubscriptions = JsonSerializer.Deserialize<SubscriptionList>(data) ?? new SubscriptionList();

        foreach (var subscription in listOfSubscriptions.Subscriptions)
        {
            Console.WriteLine($"Searching for instrumentation key {instrumentationKey} in subscription - {subscription.DisplayName}");
            HttpClient appInsightsClient = new HttpClient();
            var appInsightsUri = new Uri(@$"https://management.azure.com/subscriptions/{subscription.SubscriptionId}/providers/Microsoft.Insights/components?api-version=2015-05-01");
            appInsightsClient.BaseAddress = appInsightsUri;
            appInsightsClient.DefaultRequestHeaders.Clear();
            appInsightsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            response = await appInsightsClient.GetAsync(appInsightsUri);
            data = await response.Content.ReadAsStringAsync();
            var appInsightsInstances = JsonSerializer.Deserialize<AppInsightsList>(data) ?? new AppInsightsList();
            var instance = appInsightsInstances.AppInsights.FirstOrDefault(x => string.Equals(x.Properties.InstrumentationKey, instrumentationKey, StringComparison.OrdinalIgnoreCase));
            if (instance != null)
            {
                Console.WriteLine($"Found app insights instance {instance.Name} with instrumentation key {instance.Properties.InstrumentationKey} in subscription {subscription.DisplayName}");
                Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", $@"https://portal.azure.com/#@XYZ.COM.PQ/resource/{instance.Id}/overview");
                await Task.Delay(100);
                Environment.Exit(0);
            }
        }

        Console.WriteLine($"No app insights instance with instrumentation key - {instrumentationKey} found");
    }
}
