# Summary
This app handles all your Azure Blob Storage CRUD operation efficiently right from the command line.

Used for development:
- Azure Account
- Azure Storage Account (with connection string)
- Dotnet Sdk
- Visual Studio Code
- [Azure Storage Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurestorage)


```
.
├── README.md
├── img
│   ├── bowline.png
│   ├── diamondknot.png
│   └── half-hitch.png
├── output
├── run.sh
├── src
│   └── BlobCI
│       ├── BlobCI.csproj
│       ├── Options.cs
│       ├── Program.cs
│       ├── appsettings.yml
│       ├── bin
│       └── obj
├── watch-run.sh
└── watch.sh
```
[Options.cs](https://github.com/RobinAxelsson/ConsoleBlobApp/blob/master/src/BlobCI/Options.cs)
[Program.cs](https://github.com/RobinAxelsson/ConsoleBlobApp/blob/master/src/BlobCI/Program.cs)

# Dependencies
```xml
<!-- project file -->
  <ItemGroup>
    <PackageReference Include="Azure.Storage.Blobs" Version="12.10.0" />
    <PackageReference Include="CommandLineParser" Version="2.8.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="5.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.CommandLine" Version="5.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables" Version="5.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="5.0.0" />
    <PackageReference Include="NetEscapades.Configuration.Yaml" Version="2.1.0" />
  </ItemGroup>
```

# Connection String
Dotnet User Secrets is the most secure way to add the connection string to the application.
```shell
dotnet user-secrets init
dotnet user-secrets set CONNECTION_STRING "mystring"
dotnet user-secrets list
```
Alternatively you can add it as an Environment Variable.
```script
# bash
export CONNECTION_STRING="mystring"

# cmd
set CONNECTION_STRING="mystring"

# powershell
$Env:CONNECTION_STRING = "mystring"
```
Last option is to add it to the appsettings.yml.
```yaml
CONNECTION_STRING: "mystring"

#For azure storage emulator
UseDevelopmentStorage: "true"
```

Implementation of connection string
```csharp
//Loads all variables from multiple sources, overrides if reoccurrence.
var config = new ConfigurationBuilder()
.AddYamlFile("appsettings.yml")
.AddUserSecrets<Program>() //User secrets commands our found in README.md
.AddEnvironmentVariables()
.Build();

//Sets connection string from configuration settings if not using Azure Emulator Storage.
var cs = config["UseDevelopmentStorage"] == "true" ? "UseDevelopmentStorage=true;" : config["CONNECTION_STRING"] ??
    throw new ArgumentNullException("You need to enter a connection string");

```

# Example command
```shell
dotnet BlobCI.dll download --container mycontainer --blob-names example.png example2.png --output-folder ./output

# list all the blobs in the container.
dotnet BlobCI.dll list --container mycontainer

# deletes all blobs in the container (and deletes the container)
dotnet BlobCI.dll delete --container mycontainer --delete-all
```
# Command line parser library
https://github.com/commandlineparser/commandline#command-line-parser-library-for-clr-and-netstandard

# Microsoft Documentation
https://docs.microsoft.com/en-us/dotnet/api/overview/azure/storage.blobs-readme;;
