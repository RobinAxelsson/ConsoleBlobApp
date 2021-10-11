#!/bin/sh
pushd $HOME/Code/NET-YH-T2/ConsoleBlobApp/src/BlobCI
fswatch -or . -e .* -i \.cs$ | xargs -n1 -I{} dotnet build
popd

# fswatch -or ./Program.cs | xargs -n1 -I{} dotnet build
# fswatch -or . -e .* -i \.cs$ | xargs -n1 -I{} dotnet test
# To filter:
# Add an exclusion filter matching any string: .*.
# To include files with a given extension ext, you add an inclusion filter matching any path ending with .ext: \.ext$. In this case you need to escape the dot . to match the literal dot, then the extension ext and then matching the end of the path with $.

# Only watch extensions, -e exclude .* all -i include \.ext
# $ fswatch [options] -e .* -i \.ext
# If you want case insensitive filters -I.
