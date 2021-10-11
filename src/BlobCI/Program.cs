using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CommandLine;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace BlobCI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddYamlFile("appsettings.yml")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

            var cs = config["CONNECTION_STRING"] ??
                throw new ArgumentNullException("You need to enter a connection string");

            BlobContainerClient container = null;

            CommandLine.Parser.Default.ParseArguments<AddOptions, ListOptions>(args)
            .WithParsed<IOption>(opt =>
            {
                container = new BlobContainerClient(cs, opt.Container);
                container.CreateIfNotExists();
            })
            .WithParsed<AddOptions>(opt =>
            {
                opt.InputUris.ToList().ForEach(uri =>
                {
                    var path = File.Exists(uri) ? uri : Path.Combine(Environment.CurrentDirectory, uri);
                    var info = container.GetBlobClient(Path.GetFileName(path)).Upload(path);
                });
            });
            Console.WriteLine("End of program");
        }
    }
}