using System.Runtime.InteropServices;

namespace TestFilesGenerator.Library;

public static class FileDriveManager
{
    public static readonly string OutputDirectory;

    public static List<string> CurrentFileObjects { get; set; }

    public static long TotalSizeOfIDs { get; set; }

    static FileDriveManager()
    {
        OutputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "out");

        if (!Directory.Exists(OutputDirectory))
        {
            Directory.CreateDirectory(OutputDirectory);
        }

        CurrentFileObjects = new List<string>();
        TotalSizeOfIDs = 0;
    }

    public static void InitializeOutputStorage()
    {
        if (Directory.GetDirectories(OutputDirectory).Length > 0)
        {
            Directory.Delete(path: OutputDirectory, recursive: true);
        }
    }

    public static CustomFileStorageCleaningResult CleanBusyDiskSpace()
    {
        CustomFileStorageCleaningResult result = new ();

        uint countOfDeletedIDs = 0;

        foreach (string fileObject in CurrentFileObjects)
        {
            try
            {
                File.Delete(fileObject);
                result.SaveMessageAboutFileObject(
                    $"   >-\\-> SUCCESS DELETING: '{fileObject}'");
            }
            catch (FileNotFoundException)
            {
                result.SaveMessageAboutFileObject(
                    $"   >-/-> FAILURE DELETING: Deleting '{fileObject}' was skipped because FileNotFoundException.");
            }
            catch
            {
                result.SaveMessageAboutFileObject(
                    $"   >-/-> FAILURE DELETING: Deleting '{fileObject}' was skipped because some unknown exception.");
            }
            finally
            {
                if (!File.Exists(fileObject))
                {
                    countOfDeletedIDs++;
                }
            }
        }

        Directory.Delete(path: OutputDirectory, recursive: true);
        result.SaveInfoAboutAllFileObjects(countOfDeletedIDs);

        return result;
    }

    public static bool CanCreateNewID(CustomFileObject targetFileObject)
    {
        long availableDiskSpace = GetFreeDiskSpaceOnStorage(targetFileObject);
        long potentialTotalSize = TotalSizeOfIDs + targetFileObject.SourceFileInfo.Length;

        if (potentialTotalSize < availableDiskSpace)
        {
            return true;
        }

        return false;
    }

    public static long GetFreeDiskSpaceOnStorage(CustomFileObject targetFileObject)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string storage = RetrieveDiskName(targetFileObject).ToString() + ":\\";
            return new DriveInfo(storage).AvailableFreeSpace;
        }

        return new DriveInfo(OutputDirectory).AvailableFreeSpace;
    }

    private static char RetrieveDiskName(CustomFileObject targetFileObject)
    {
        return targetFileObject.TargetFileInfo.FullName[0];
    }
}

public record CustomFileStorageCleaningResult
{
    public List<string> OutputMessages { get; private set; }

    public CustomFileStorageCleaningResult()
    {
        this.OutputMessages = new List<string>() { "\n\n   >>> Cleaning ...\n\n" };
    }

    public void SaveMessageAboutFileObject(string message) =>
        this.OutputMessages.Add(message);
    public void SaveInfoAboutAllFileObjects(uint cleanedFilesCount) =>
        this.OutputMessages.Add($"\n\n   [INFO]: Deleted {cleanedFilesCount} files.\n");
}
