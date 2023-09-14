using static System.Console;
using static TestFilesGenerator.Library.MediaStorage;

namespace TestFilesGenerator.Library;

public class MediaObject
{
    public FileInfo SourceMediaFileInfo { get; }
    public FileInfo TargetMediaFileInfo { get; }

    public MediaObject(string mediaPath, string mediaSource)
    {
        try
        {
            TargetMediaFileInfo = new FileInfo(mediaPath);
            SourceMediaFileInfo = new FileInfo(mediaSource);
        }
        catch (FileNotFoundException)
        {
            WriteLine("[ERROR]: Specified file is not found. The media object cannot be created.");
        }
    }

    public void Clone()
    {
        CreateMediaObjectCopy();
        ShowStorageStatus();
    }

    private void CreateMediaObjectCopy()
    {
        string sourceMediaFile = SourceMediaFileInfo.FullName;
        string targetMediaFile = TargetMediaFileInfo.FullName;
        string targetMediaName = TargetMediaFileInfo.Name;

        if (CanCreateNewID(this))
        {
            File.Copy(sourceMediaFile, targetMediaFile, true);

            MediaFilesList.Add(targetMediaFile);
            TotalSizeOfIDs += TargetMediaFileInfo.Length;

            OutCloningResult(targetMediaName);
        }
        else
        {
            OutCloningError(targetMediaName);
        }
    }

    private void OutCloningResult(string idName)
    {
        WriteLine("{0}\n", GeneratorService.BuildSeparator());
        WriteLine($"   [{DateTimeService.GetDateAndTimeDefaultString()}]: {idName}\n");
        WriteLine("   [!] Available Space: {0} bytes.", GetFreeDiskSpaceOnStorage(this));
    }

    private void OutCloningError(string idName)
    {
        WriteLine($"   [ERROR]: {idName} cannot be created via cloning.");
    }
}
