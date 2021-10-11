#!/usr/bin/env bash
pushd $HOME/Code/NET-YH-T2/ConsoleBlobApp/src/BlobCI
dotnet bin/Debug/netcoreapp3.1/BlobCI.dll add -c CI-Container -u ./img/test.txt
echo -----------------
