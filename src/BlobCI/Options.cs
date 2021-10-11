using System.Collections.Generic;
using CommandLine;
//https://github.com/commandlineparser/commandline#command-line-parser-library-for-clr-and-netstandard

namespace BlobCI
{
    interface IOption
    {
        public string Container { get; set; }
    }
    [Verb("download", HelpText = "Downloads input file names to given output path.")]
    class DownloadOptions : IOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to delete resources.")]
        public string Container { get; set; }
        [Option('b', "blob-names", Required = true, HelpText = "Input blob names to be downloaded")]
        public IEnumerable<string> BlobNames { get; set; }

        [Option('o', "output-folder", Required = false, HelpText = "Directory were files will be outputted")]
        public string OutputDirectory { get; set; }
    }
    [Verb("add", HelpText = "Add file content to named blob container.")]
    class AddOptions : IOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to add resources.")]
        public string Container { get; set; }
        [Option('u', "uri", Required = true, HelpText = "Input files to be added.")]
        public IEnumerable<string> InputUris { get; set; }
    }
    [Verb("list", HelpText = "Lists file resources in named blob container.")]
    class ListOptions : IOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to list resources.")]
        public string Container { get; set; }
    }
    [Verb("update", HelpText = "Lists file resources in named blob container.")]
    class UpdateOptions : IOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to update resources.")]
        public string Container { get; set; }
        [Option('u', "uri", Required = true, HelpText = "Input uri to be updated.")]
        public IEnumerable<string> InputUris { get; set; }
    }
    [Verb("delete", HelpText = "Deletes file resources in named blob container.")]
    class DeleteOptions : IOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to delete resources from.")]
        public string Container { get; set; }
        [Option('b', "blob-names", Required = false, HelpText = "Input blob names to delete")]
        public IEnumerable<string> BlobNames { get; set; }
        [Option("delete-all", Required = false, HelpText = "Deletes all blobs (and container)")]
        public bool DeleteContainer { get; set; }
    }
}