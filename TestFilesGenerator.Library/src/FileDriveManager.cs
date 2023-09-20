using static System.Console;

namespace TestFilesGenerator.Library;

public static class FileDriveManager
{
    public static List<string> CurrentFileObjects { get; set; }
    public static long TotalSizeOfIDs { get; set; }
    public static readonly string OutputDirectory;

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
        WriteLine("   >>> Initializing the output storage for IDs ...\n");

        if (Directory.GetDirectories(OutputDirectory).Length > 0)
        {
            Directory.Delete(path: OutputDirectory, recursive: true);
            WriteLine($"   >-\\-> Deleted all existing content from the output directory '{OutputDirectory}'.");
        }

        WriteLine();
    }

    public static void ShowStorageStatus()
    {
        WriteLine();
        WriteLine("   [?] Total Size of IDs : {0} bytes.", TotalSizeOfIDs);
        WriteLine("   [?] Total Count of IDs : {0} files.", CurrentFileObjects.Count);
        WriteLine();
    }

    public static void CleanBusyDiskSpace()
    {
        uint countOfDeletedIDs = 0;

        WriteLine("\n\n   >>> Cleaning ...\n\n");

        foreach (var fileObject in CurrentFileObjects)
        {
            try
            {
                File.Delete(fileObject);
                WriteLine("   >-\\-> SUCCESS DELETING: '{0}'", fileObject);
            }
            catch (FileNotFoundException)
            {
                WriteLine("   >-/-> FAILURE DELETING: Deleting '{0}' was skipped because FileNotFoundException.", fileObject);
            }
            catch
            {
                WriteLine("   >-/-> FAILURE DELETING: Deleting '{0}' was skipped because some unknown exception.", fileObject);
            }
            finally
            {
                if (!File.Exists(fileObject))
                {
                    countOfDeletedIDs++;
                }
            }
        }

        WriteLine("\n\n   [SUCCESS]: There were deleted {0} files from {1}.\n", countOfDeletedIDs, CurrentFileObjects.Count);
        Directory.Delete(path: OutputDirectory, recursive: true);
    }

    public static bool CanCreateNewID(CustomFileObject targetFileObject)
    {
        long availableDiskSpace = GetFreeDiskSpaceOnStorage(targetFileObject);
        long potentialTotalSize = TotalSizeOfIDs + targetFileObject.SourceFileInfo.Length;

        if (potentialTotalSize < availableDiskSpace)
        {
            return true;
        }

        Write("[CAUTION]: Cannot generate a new instance of the file. ");
        WriteLine("There is no enough available space on the storage.");

        return false;
    }

    public static long GetFreeDiskSpaceOnStorage(CustomFileObject targetFileObject)
    {
        string storage = RetrieveDiskName(targetFileObject).ToString() + ":\\";
        return new DriveInfo(storage).AvailableFreeSpace;
    }

    private static char RetrieveDiskName(CustomFileObject targetFileObject)
    {
        return targetFileObject.TargetFileInfo.FullName[0];
    }
}
