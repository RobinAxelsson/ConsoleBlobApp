using System;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.Extensions.Configuration;
using CommandLine;
using System.Linq;

namespace BlobCI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Loads all variables from multiple sources, overrides if reoccurrence.
            var config = new ConfigurationBuilder()
            .AddYamlFile("appsettings.yml")
            .AddUserSecrets<Program>() //User secrets commands our found in README.md
            .AddEnvironmentVariables()
            .Build();

            //Sets connection string from configuration settings if not using Azure Emulator Storage.
            var cs = config["UseDevelopmentStorage"] == "true" ? "UseDevelopmentStorage=true;" : config["CONNECTION_STRING"] ??
                throw new ArgumentNullException("You need to enter a connection string");

            BlobContainerClient container = null;

            //Parses all the input arguments from general command line syntax to NET-objects where properties can be used and specified.
            //An Action verb specifies which Option-Class used: eg "add", "download", "list".
            CommandLine.Parser.Default.ParseArguments<AddOptions, ListOptions, DownloadOptions, DeleteOptions, UpdateOptions>(args)

            //Gets the blobContainer for all actions
            .WithParsed<IOption>(opt =>
            {
                container = new BlobContainerClient(cs, opt.Container);
                container.CreateIfNotExists();
            })

            //"add" action uses input file paths to upload to container
            .WithParsed<AddOptions>(opt =>
            {
                opt.InputUris.ToList().ForEach(uri =>
                {
                    var path = File.Exists(uri) ? uri : Path.Combine(Environment.CurrentDirectory, uri);
                    var info = container.GetBlobClient(Path.GetFileName(path)).Upload(path);
                });
            })

            //"list" action prints all blob names to standard out. 
            .WithParsed<ListOptions>(opt => container.GetBlobs().ToList().ForEach(b => Console.WriteLine(b.Name)))

            //"download" action adds blob(s) to specified output folder (or current folder);
            .WithParsed<DownloadOptions>(opt =>
            {
                string outpoutDir = Directory.Exists(opt.OutputDirectory) ? opt.OutputDirectory :
                Directory.Exists(Path.Combine(Environment.CurrentDirectory, opt.OutputDirectory)) ? Path.Combine(Environment.CurrentDirectory, opt.OutputDirectory) :
                Environment.CurrentDirectory;

                opt.BlobNames.ToList().ForEach(name =>
                {
                    var blobClient = container.GetBlobClient(name);
                    Console.WriteLine(outpoutDir + name);
                    blobClient.DownloadTo(Path.Combine(outpoutDir + "/" + name));
                });
            })
            .WithParsed<DeleteOptions>(opt =>
            {
                if (opt.DeleteContainer) //Deletes all blobs in one go by deleting the container.
                    container.Delete();
                else
                    opt.BlobNames.ToList().ForEach(name => container.DeleteBlob(name));
            })
            .WithParsed<UpdateOptions>(opt =>
            {
                opt.InputUris.ToList().ForEach(uri =>
                {
                    var fileName = Path.GetFileName(uri); //"update" action deletes the current blob first then uploads the local blob.
                    container.DeleteBlob(fileName);

                    var path = File.Exists(uri) ? uri : Path.Combine(Environment.CurrentDirectory, uri);
                    var info = container.GetBlobClient(fileName).Upload(path);
                });
            });
            Console.WriteLine("End of program");
        }
    }
}