using static System.Console;

namespace TestFilesGenerator.Library;

public class CustomFileObject
{
    public FileInfo SourceFileInfo { get; }
    public FileInfo TargetFileInfo { get; }

    public CustomFileObject(string targetFileName, string sourceFileName)
    {
        try
        {
            TargetFileInfo = new FileInfo(targetFileName);
            SourceFileInfo = new FileInfo(sourceFileName);
        }
        catch (FileNotFoundException)
        {
            WriteLine("[ERROR]: Specified file is not found. The file object cannot be created.");
        }
    }

    public void Clone()
    {
        CreateFileObjectCopy();
        FileDriveManager.ShowStorageStatus();
    }

    private void CreateFileObjectCopy()
    {
        string sourceFile = SourceFileInfo.FullName;
        string targetFile = TargetFileInfo.FullName;
        string targetName = TargetFileInfo.Name;

        if (FileDriveManager.CanCreateNewID(this))
        {
            File.Copy(sourceFile, targetFile, true);

            FileDriveManager.CurrentFileObjects.Add(targetFile);
            FileDriveManager.TotalSizeOfIDs += TargetFileInfo.Length;

            OutCloningResult(targetName);
        }
        else
        {
            OutCloningError(targetName);
        }
    }

    private void OutCloningResult(string idName)
    {
        WriteLine("{0}\n", GeneratorService.BuildSeparator());
        WriteLine($"   [{DateTimeService.GetDateAndTimeDefaultString()}]: {idName}\n");
        WriteLine("   [!] Available Space: {0} bytes.", FileDriveManager.GetFreeDiskSpaceOnStorage(this));
    }

    private void OutCloningError(string idName)
    {
        WriteLine($"   [ERROR]: {idName} cannot be created via cloning.");
    }
}
