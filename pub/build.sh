#!/bin/bash

dotnet publish -o ./linux-x64 -r linux-x64  -c Release --self-contained true --use-current-runtime -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true ../src/AHashPassGen/AHashPassGen.csproj
dotnet publish -o ./win-x64 -r win-x64  -c Release --self-contained true --use-current-runtime -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true ../src/AHashPassGen/AHashPassGen.csproj

rm ./linux-x64/*.pdb
rm ./win-x64/*.pdb