//-----------------------------------------------------------------------
// <copyright file="CustomFileStorage.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Library;

using System.Runtime.InteropServices;
using System.Xml.Serialization;

/// <summary>
/// Represents a storage for custom file collections.
/// </summary>
public class CustomFileStorage
{
    private static readonly string StorageOutputDirectory;
    private static readonly string StorageConfiguration;

    /// <summary>
    /// Initializes static members of the <see cref="CustomFileStorage"/> class.
    /// </summary>
    static CustomFileStorage()
    {
        StorageOutputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "out");
        StorageConfiguration = Path.Combine(Directory.GetCurrentDirectory(), "config.xml");

        if (!Directory.Exists(StorageOutputDirectory))
        {
            Directory.CreateDirectory(StorageOutputDirectory);
        }

        if (Directory.GetDirectories(StorageOutputDirectory).Length > 0)
        {
            Directory.Delete(path: StorageOutputDirectory, recursive: true);
        }

        CurrentFileObjects = new List<string>();
        TotalSizeOfIDs = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileStorage"/> class.
    /// </summary>
    public CustomFileStorage()
    {
        this.UserFileCollections = this.RetrieveFileCollections();
    }

    /// <summary>
    /// Gets or sets current files located into the storage.
    /// </summary>
    public static List<string> CurrentFileObjects { get; set; }

    /// <summary>
    /// Gets or sets the total size of files located into the storage in bytes.
    /// </summary>
    public static long TotalSizeOfIDs { get; set; }

    /// <summary>
    /// Gets custom file collections located into the storage.
    /// </summary>
    public List<CustomFileCollection> UserFileCollections { get; }

    /// <summary>
    /// Checks that a custom file object can be located into the storage.
    /// </summary>
    /// <param name="targetFileObject">A custom file object to be located into the storage.</param>
    /// <returns>If 'true' then a custom file object can be located into the storage else can't.</returns>
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

    /// <summary>
    /// Checks free disk space on the storage drive in bytes.
    /// </summary>
    /// <param name="targetFileObject">A custom file object.</param>
    /// <returns>Count of free disk space in bytes.</returns>
    public static long GetFreeDiskSpaceOnStorage(CustomFileObject targetFileObject)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string storage = RetrieveDiskName(targetFileObject).ToString() + ":\\";
            return new DriveInfo(storage).AvailableFreeSpace;
        }

        return new DriveInfo(StorageOutputDirectory).AvailableFreeSpace;
    }

    /// <summary>
    /// Gets the directory path for a custom file collection via its alias.
    /// </summary>
    /// <param name="collectionAlias">Alias of a custom file collection.</param>
    /// <returns>Directory path for the custom file collection.</returns>
    public static string GetCustomFileCollectionDirectory(string collectionAlias)
    {
        string directory = Path.Combine(StorageOutputDirectory, collectionAlias);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
    }

    /// <summary>
    /// Executes cleanup the storage via deleting all custom file collections with file objects.
    /// </summary>
    /// <returns>A record object with info about made cleanup results.</returns>
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

        Directory.Delete(path: StorageOutputDirectory, recursive: true);
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

        using (FileStream stream = File.Open(StorageConfiguration, FileMode.Open))
        {
            collection = (List<CustomFileCollection>)serializer.Deserialize(stream);
        }

        return collection;
    }
}

/// <summary>
/// Represents a state of made storage cleanup.
/// </summary>
public record CustomFileStorageCleaningResult
{
    /// <summary>
    /// Gets the list of information messages to be printed to output device.
    /// </summary>
    public List<string> OutputMessages { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileStorageCleaningResult"/> class.
    /// </summary>
    public CustomFileStorageCleaningResult()
    {
        this.OutputMessages = new List<string>() { "\n\n   >>> Cleaning ...\n\n" };
    }

    /// <summary>
    /// Save information about a step of storage cleanup.
    /// </summary>
    /// <param name="message">A string with a result message.</param>
    public void SaveMessageAboutFileObject(string message) =>
        this.OutputMessages.Add(message);

    /// <summary>
    /// Save information about the total count of files cleaned from the storage.
    /// </summary>
    /// <param name="cleanedFilesCount">The total count of files cleaned from the storage.</param>
    public void SaveInfoAboutAllFileObjects(uint cleanedFilesCount) =>
        this.OutputMessages.Add($"\n\n   [INFO]: Deleted {cleanedFilesCount} files.\n");
}
