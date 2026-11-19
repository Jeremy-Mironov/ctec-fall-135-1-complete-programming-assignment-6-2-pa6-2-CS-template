/*
 * name: Eremei Mironov
 * course: ctec 135
 * assignment: programming assignment 6-2, task 1
 * date: 11/18/2026
 */

using Microsoft.Extensions.Configuration;

namespace ConfigDemo;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("***** Config File Demo *****");
        Console.WriteLine();

        // get provider and connection string from appsettings.json
        var (provider, connectionString) = GetProviderFromConfiguration();

        // print both values
        Console.WriteLine($"Provider: {provider}");
        Console.WriteLine($"Connection String: {connectionString}");
    }

    // reads the config file and returns the provider name and the connection string
    static (string Provider, string ConnectionString) GetProviderFromConfiguration()
    {
        // build the config object from the appsettings.json file
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        // return the two values from the file
        return (config["ProviderName"], config["SqlServer:ConnectionString"]);
    }
}
