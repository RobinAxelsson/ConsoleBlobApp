using System.Collections.Generic;
using CommandLine;
//https://github.com/commandlineparser/commandline#command-line-parser-library-for-clr-and-netstandard

namespace BlobCI
{
    interface IOption
    {
        public string Container { get; set; }
    }
    interface IURIOption
    {
        public IEnumerable<string> InputUris { get; set; }
    }
    [Verb("add", HelpText = "Add file content to named blob container.")]
    class AddOptions : IOption, IURIOption
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
    class UpdateOptions : IOption, IURIOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to update resources.")]
        public string Container { get; set; }
        [Option('u', "uri", Required = true, HelpText = "Input uri to be updated.")]
        public IEnumerable<string> InputUris { get; set; }
    }
    class DeleteOptions : IOption, IURIOption
    {
        [Option('c', "container", Required = true, HelpText = "Container name to delete resources.")]
        public string Container { get; set; }
        [Option('u', "uri", Required = true, HelpText = "Input uri to be deleted.")]
        public IEnumerable<string> InputUris { get; set; }
    }
}