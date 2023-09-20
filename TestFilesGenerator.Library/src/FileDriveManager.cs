using static System.Console;

namespace TestFilesGenerator.Library;

public static class FileDriveManager
{
    public static List<string> CurrentFileObjects { get; set; }
    public static long TotalSizeOfIDs { get; set; }
    public static string OutputDirectory { get; }

    static FileDriveManager()
    {
        OutputDirectory = GetCommonOutputDirectory();
        CurrentFileObjects = new List<string>();
        TotalSizeOfIDs = 0;
    }

    public static void InitializeOutputStorage()
    {
        WriteLine("   >>> Initializing the output storage for IDs ...");
        WriteLine();

        string[] oldCollections = Directory.GetDirectories(OutputDirectory);

        foreach (var oldCollection in oldCollections)
        {
            WriteLine("   >-\\-> \"{0}\" was deleted with its content.", oldCollection);
            Directory.Delete(oldCollection, true);
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

        foreach (var mediaFile in CurrentFileObjects)
        {
            try
            {
                File.Delete(mediaFile);
                WriteLine("   >-\\-> SUCCESS DELETING: '{0}'", mediaFile);
            }
            catch (FileNotFoundException)
            {
                WriteLine("   >-/-> FAILURE DELETING: Deleting '{0}' was skipped because FileNotFoundException.", mediaFile);
            }
            catch
            {
                WriteLine("   >-/-> FAILURE DELETING: Deleting '{0}' was skipped because some unknown exception.", mediaFile);
            }
            finally
            {
                if (!File.Exists(mediaFile))
                {
                    countOfDeletedIDs++;
                }
            }
        }

        WriteLine("\n\n   [SUCCESS]: There were deleted {0} files from {1}.\n", countOfDeletedIDs, CurrentFileObjects.Count);
    }

    public static bool CanCreateNewID(MediaObject targetMediaFile)
    {
        long availableDiskSpace = GetFreeDiskSpaceOnStorage(targetMediaFile);
        long potentialTotalSize = TotalSizeOfIDs + targetMediaFile.SourceMediaFileInfo.Length;

        if (potentialTotalSize < availableDiskSpace)
        {
            return true;
        }

        Write("[CAUTION]: Cannot generate a new instance of the file. ");
        WriteLine("There is no enough available space on the storage.");

        return false;
    }

    public static long GetFreeDiskSpaceOnStorage(MediaObject targetMediaFile)
    {
        string storage = RetrieveDiskName(targetMediaFile).ToString() + ":\\";
        return new DriveInfo(storage).AvailableFreeSpace;
    }

    private static char RetrieveDiskName(MediaObject targetMediaFile)
    {
        return targetMediaFile.TargetMediaFileInfo.FullName[0];
    }

    private static string GetCommonOutputDirectory()
    {
        string outputDirectory = @".\out\";

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return outputDirectory;
    }
}
