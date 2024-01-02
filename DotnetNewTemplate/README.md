# VerticalSliceArchitectureGenerator

A generator to produce a vertical slice architecture from API to integration coded tests

# DotnetNewTemplate

## By CLI

### Install

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-install

```
dotnet new install <PATH|NUGET_ID>  [--interactive] [--add-source|--nuget-source <SOURCE>] [--force] 
    [-d|--diagnostics] [--verbosity <LEVEL>] [-h|--help]
```

For example: 

`dotnet new install . --force`

### Apply

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new

```
dotnet new <TEMPLATE> [--dry-run] [--force] [-lang|--language {"C#"|"F#"|VB}]
    [-n|--name <OUTPUT_NAME>] [-f|--framework <FRAMEWORK>] [--no-update-check]
    [-o|--output <OUTPUT_DIRECTORY>] [--project <PROJECT_PATH>]
    [-d|--diagnostics] [--verbosity <LEVEL>] [Template options]

dotnet new -h|--help
```

For example: 

`dotnet new vsa_generator -n MyFeature -o MyFeatureFolder -e MyEntity`

### Uninstall

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-uninstall

```
dotnet new uninstall <PATH|NUGET_ID> 
    [-d|--diagnostics] [--verbosity <LEVEL>] [-h|--help]
```

For example: 

`dotnet new uninstall .`

