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
            var config = new ConfigurationBuilder()
            .AddYamlFile("appsettings.yml")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

            var cs = config["UseDevelopmentStorage"] == "true" ? "UseDevelopmentStorage=true;" : config["CONNECTION_STRING"] ??
                throw new ArgumentNullException("You need to enter a connection string");

            BlobContainerClient container = null;

            CommandLine.Parser.Default.ParseArguments<AddOptions, ListOptions, DownloadOptions, DeleteOptions, UpdateOptions>(args)
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
            })
            .WithParsed<ListOptions>(opt => container.GetBlobs().ToList().ForEach(b => Console.WriteLine(b.Name)))
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
                if (opt.DeleteContainer)
                    container.Delete();
                else
                    opt.BlobNames.ToList().ForEach(name => container.DeleteBlob(name));
            })
            .WithParsed<UpdateOptions>(opt =>
            {
                opt.InputUris.ToList().ForEach(uri =>
                {
                    var fileName = Path.GetFileName(uri);
                    container.DeleteBlob(fileName);

                    var path = File.Exists(uri) ? uri : Path.Combine(Environment.CurrentDirectory, uri);
                    var info = container.GetBlobClient(fileName).Upload(path);
                });
            });
            Console.WriteLine("End of program");
        }
    }
}