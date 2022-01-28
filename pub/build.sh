#!/bin/bash

dotnet publish -o ./linux-x64 -r linux-x64  -c Release --self-contained true  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:PublishReadyToRun=true ../src/AHashPassGen/AHashPassGen.csproj
dotnet publish -o ./win-x64 -r win-x64  -c Release --self-contained true  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:PublishReadyToRun=true ../src/AHashPassGen/AHashPassGen.csproj
#dotnet publish -o ./win-x64-dist -r win-x64 --self-contained false -c Release ../src/AHashPassGen/AHashPassGen.csproj


#--use-current-runtime
#-p:PublishTrimmed=true

rm ./linux-x64/*.pdb
rm ./win-x64/*.pdb
rm ./win-x64-dist/*.pdb