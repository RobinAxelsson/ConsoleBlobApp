using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;


namespace BlobCI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddYamlFile("appsettings.yml")
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddUserSecrets<Program>()
            .Build();
            //BlobServiceClient blobServiceClient = new BlobServiceClient(config["CONNECTION_STRING"]);
            Console.WriteLine(config["CONNECTION_STRING"]);
            Console.WriteLine("Test variable secret=" + config["secret"]);
        }
    }
}
