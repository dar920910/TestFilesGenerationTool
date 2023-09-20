FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy
WORKDIR /usr/local/src/TestFilesGenerationTool

COPY TestFilesGenerator.Library/ TestFilesGenerator.Library/
COPY TestFilesGenerator.App.CLI/ TestFilesGenerator.App.CLI/

RUN dotnet publish TestFilesGenerator.App.CLI/TestFilesGenerator.App.CLI.csproj --output "/usr/local/bin/TestFilesGenerationTool/CLI/" --configuration "Release" --use-current-runtime --no-self-contained
WORKDIR /usr/local/bin/TestFilesGenerationTool/CLI/