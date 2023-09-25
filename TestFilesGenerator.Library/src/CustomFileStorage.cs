using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace TestFilesGenerator.Library;

public class CustomFileStorage
{
    static CustomFileStorage()
    {
        storageOutputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "out");
        storageConfiguration = Path.Combine(Directory.GetCurrentDirectory(), "config.xml");

        if (!Directory.Exists(storageOutputDirectory))
        {
            Directory.CreateDirectory(storageOutputDirectory);
        }

        if (Directory.GetDirectories(storageOutputDirectory).Length > 0)
        {
            Directory.Delete(path: storageOutputDirectory, recursive: true);
        }

        CurrentFileObjects = new List<string>();
        TotalSizeOfIDs = 0;
    }

    public CustomFileStorage()
    {
        this.UserFileCollections = RetrieveFileCollections();
    }

    private static readonly string storageOutputDirectory;
    private static readonly string storageConfiguration;

    public static List<string> CurrentFileObjects { get; set; }

    public static long TotalSizeOfIDs { get; set; }

    public List<CustomFileCollection> UserFileCollections { get; }

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

        return new DriveInfo(storageOutputDirectory).AvailableFreeSpace;
    }

    public static string GetCustomFileCollectionDirectory(string collectionAlias)
    {
        string directory = Path.Combine(storageOutputDirectory, collectionAlias);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
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

        Directory.Delete(path: storageOutputDirectory, recursive: true);
        result.SaveInfoAboutAllFileObjects(countOfDeletedIDs);

        return result;
    }

    private static char RetrieveDiskName(CustomFileObject targetFileObject)
    {
        return targetFileObject.TargetFileInfo.FullName[0];
    }

    private List<CustomFileCollection> RetrieveFileCollections()
    {
        var collection = new List<CustomFileCollection>();
        var serializer = new XmlSerializer(typeof(List<CustomFileCollection>));

        using (FileStream stream = File.Open(storageConfiguration, FileMode.Open))
        {
            collection = (List<CustomFileCollection>)serializer.Deserialize(stream);
        }

        return collection;
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
