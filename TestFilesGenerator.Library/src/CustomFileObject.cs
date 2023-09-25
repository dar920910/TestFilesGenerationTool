//-----------------------------------------------------------------------
// <copyright file="CustomFileObject.cs" company="DemoProjectsWorkshop">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Library;

public class CustomFileObject
{
    private string resultOutputMessage;

    public FileInfo SourceFileInfo { get; }

    public FileInfo TargetFileInfo { get; }

    public CustomFileObject(string targetFileName, string sourceFileName)
    {
        this.resultOutputMessage = default;

        try
        {
            this.TargetFileInfo = new FileInfo(targetFileName);
            this.SourceFileInfo = new FileInfo(sourceFileName);
        }
        catch (FileNotFoundException)
        {
            this.resultOutputMessage = "[ERROR]: Specified file is not found. The file object cannot be created.";
        }
    }

    /// <summary>
    /// Create a target file object from its source file.
    /// </summary>
    /// <returns>Result record with information about made cloning.</returns>
    public CustomFileObjectCreationResult Clone()
    {
        string sourceFile = this.SourceFileInfo.FullName;
        string targetFile = this.TargetFileInfo.FullName;
        string targetName = this.TargetFileInfo.Name;

        if (FileDriveManager.CanCreateNewID(this))
        {
            File.Copy(sourceFile, targetFile, true);

            FileDriveManager.CurrentFileObjects.Add(targetFile);
            FileDriveManager.TotalSizeOfIDs += this.TargetFileInfo.Length;

            this.resultOutputMessage += this.GetCloningResultMessage(targetName);
        }
        else
        {
            this.resultOutputMessage += GetCloningErrorMessage(targetName);
        }

        FileDriveManager.ShowStorageStatus();

        return new CustomFileObjectCreationResult(this.resultOutputMessage);
    }

    private static string GetCloningErrorMessage(string idName) =>
        $"   [ERROR]: {idName} cannot be created via cloning.";

    private string GetCloningResultMessage(string idName) =>
        $"   [{DateTimeService.GetDateAndTimeDefaultString()}]: {idName}\n" +
        $"   [!] Available Space: {FileDriveManager.GetFreeDiskSpaceOnStorage(this)} bytes.";
}

public record CustomFileObjectCreationResult
{
    public string OutputMessage { get; }
    public CustomFileObjectCreationResult(string message)
    {
        this.OutputMessage = message;
    }
}
