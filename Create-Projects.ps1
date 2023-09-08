Write-Host "`nStep 1: Creating a New Solution`n"

dotnet new sln
dotnet new gitignore


Write-Host "`nStep 2: Creating Projects from Templates`n"

dotnet new classlib --name "TestFilesGenerator.Library" --framework "net6.0"
dotnet new nunit --name "TestFilesGenerator.Testing" --framework "net6.0"
dotnet new console --name "TestFilesGenerator.App.CLI" --framework "net6.0"


Write-Host "`nStep 3: Adding References to Projects`n"

dotnet add "TestFilesGenerator.Testing" reference "TestFilesGenerator.Library"
dotnet add "TestFilesGenerator.App.CLI" reference "TestFilesGenerator.Library"


Write-Host "`nStep 4: Adding Projects to the Solution`n"

dotnet sln add "TestFilesGenerator.Library"
dotnet sln add "TestFilesGenerator.Testing"
dotnet sln add "TestFilesGenerator.App.CLI"


Write-Host "`nStep 5: Displaying Projects from the Solution`n"

dotnet sln list
