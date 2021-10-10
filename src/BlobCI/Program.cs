using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;

namespace BlobCI
{
    class Program
    {
        private static BlobServiceClient _blobServiceClient;
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddYamlFile("appsettings.yml")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

            var cs = config["CONNECTION_STRING"] ??
                throw new ArgumentNullException("You need to enter a connection string");
            var containerName = config["container-name"] ??
                throw new ArgumentNullException("You need to add container name");

            _blobServiceClient = new BlobServiceClient(config["CONNECTION_STRING"]);

            BlobContainerClient containerClient = await TryGetBlobContainerAsync(containerName) ??
                throw new ArgumentNullException("Container can't be null.");


            var path = File.Exists(args[0]) ? args[0] : Path.Combine(Environment.CurrentDirectory, args[0]);

            BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(path));
            Console.WriteLine(path);

            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    //await blobClient.UploadAsync()
                    Console.WriteLine(temp.GetString(b));
                }
            }
        }
        private static async Task<BlobContainerClient> TryGetBlobContainerAsync(string name)
        {
            BlobContainerClient containerClient;
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(name);
            }
            catch (Azure.RequestFailedException ex)
            {
                Console.WriteLine("Container does not Exist");
                Console.WriteLine(ex.GetType().ToString());
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(name);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return containerClient;
        }
    }
}
