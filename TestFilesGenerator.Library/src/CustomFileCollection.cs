//-----------------------------------------------------------------------
// <copyright file="CustomFileCollection.cs" company="Demo Projects Workshop">
// Copyright (c) Demo Projects Workshop. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TestFilesGenerator.Library;

using System.Xml.Serialization;

/// <summary>
/// Represents a custom collection of file objects based on a file template.
/// </summary>
public class CustomFileCollection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileCollection"/> class.
    /// </summary>
    public CustomFileCollection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileCollection"/> class.
    /// </summary>
    /// <param name="alias">User-defined name of the custom collection.</param>
    /// <param name="source">User-defined file template of file objects in the collection.</param>
    /// <param name="count">User-defined count of file objects to be cloned from the file template in the collection.</param>
    /// <param name="hasRandomFileNames">Generate file objects with random names if 'true' else use both an alias and a count.</param>
    /// <param name="randomFileNameLength">User-defined length of random names for file objects int the collection.</param>
    public CustomFileCollection(
        string alias, string source, uint count, bool hasRandomFileNames = false, byte randomFileNameLength = 0)
    {
        this.Name = alias;
        this.SourceFileObject = source;
        this.CountOfFileObjects = count;
        this.HasRandomFileNames = hasRandomFileNames;
        this.RandomFileNameLength = randomFileNameLength;
    }

    /// <summary>
    /// Gets or sets the user-defined name of a custom file collection.
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the file path as the template for file objects in the collection.
    /// </summary>
    [XmlAttribute("SourceFileObject")]
    public string SourceFileObject { get; set; }

    /// <summary>
    /// Gets or sets the count of file objects to be created in the collection.
    /// </summary>
    [XmlAttribute("CountOfFileObjects")]
    public uint CountOfFileObjects { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether random names should be assigned to file objects in the collection.
    /// </summary>
    [XmlAttribute("HasRandomFileNames")]
    public bool HasRandomFileNames { get; set; }

    /// <summary>
    /// Gets or sets the fixed length of random names for all file objects in the collection.
    /// </summary>
    [XmlAttribute("RandomFileNameLength")]
    public byte RandomFileNameLength { get; set; }

    /// <summary>
    /// Retrieves names of possible file objects by collection's parameters.
    /// </summary>
    /// <param name="alias">The name of a collection.</param>
    /// <param name="source">The source filename.</param>
    /// <param name="count">The count of file objects.</param>
    /// <returns>The list of names for possible file objects.</returns>
    public static List<string> RetrieveFileObjectNames(string alias, string source, uint count)
    {
        List<string> fileObjectNames = new ();

        for (uint number = 1; number <= count; number++)
        {
            string idName = GenerateCustomIdName(alias, source, number);
            fileObjectNames.Add(idName);
        }

        return fileObjectNames;
    }

    /// <summary>
    /// Retrieves file objects from the custom file collection.
    /// </summary>
    /// <returns>The list of custom file objects.</returns>
    public List<CustomFileObject> RetrieveFileObjects()
    {
        var fileObjects = new List<CustomFileObject>();
        CustomFileObject[] arrayOfFileObjects = this.GenerateFileObjects();

        foreach (var fileObject in arrayOfFileObjects)
        {
            fileObjects.Add(fileObject);
        }

        return fileObjects;
    }

    private static string GenerateCustomIdName(string alias, string filename, uint number) =>
        $"{alias}_{new CollectionFileObjectNumberId(number).Create()}{GetFileExtensionFromSource(filename)}";

    private static string GetFileExtensionFromSource(string source) => new FileInfo(source).Extension;

    private CustomFileObject[] GenerateFileObjects()
    {
        var arrayOfFileObjects = new CustomFileObject[this.CountOfFileObjects];

        for (uint idCounter = 0; idCounter < arrayOfFileObjects.Length; idCounter++)
        {
            string fileName = this.HasRandomFileNames ? this.GenerateRandomIdName() : GenerateCustomIdName(this.Name, this.SourceFileObject, idCounter + 1);
            string filePath = this.GenerateTargetIdPath(fileName);
            arrayOfFileObjects[idCounter] = new CustomFileObject(filePath, this.SourceFileObject);
        }

        return arrayOfFileObjects;
    }

    private string GenerateTargetIdPath(string targetIdName)
    {
        string targetDirectory = CustomFileStorage.GetCustomFileCollectionDirectory(this.Name);
        return Path.Combine(targetDirectory, targetIdName);
    }

    private string GenerateRandomIdName()
    {
        return new RandomFileObjectName(this.RandomFileNameLength).Create() + GetFileExtensionFromSource(this.SourceFileObject);
    }
}

/// <summary>
/// Represents a numeric ID for a file object from a custom file collection.
/// </summary>
public class CollectionFileObjectNumberId
{
    private const uint ValueLevel1 = 1_000_000;
    private const uint ValueLevel2 = 100_000;
    private const uint ValueLevel3 = 10_000;
    private const uint ValueLevel4 = 1_000;
    private const uint ValueLevel5 = 100;
    private const uint ValueLevel6 = 10;

    private readonly uint fileObjectNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionFileObjectNumberId"/> class.
    /// </summary>
    /// <param name="number">Number of a file object in a custom file collection.</param>
    public CollectionFileObjectNumberId(uint number)
    {
        this.fileObjectNumber = number;
    }

    /// <summary>
    /// Create a numeric ID for a file object.
    /// </summary>
    /// <returns>Numeric ID of a file object.</returns>
    public string Create()
    {
        string targetNumber;

        if (this.fileObjectNumber < ValueLevel6)
        {
            targetNumber = AddZeroes(6);
        }
        else if (this.fileObjectNumber < ValueLevel5)
        {
            targetNumber = AddZeroes(5);
        }
        else if (this.fileObjectNumber < ValueLevel4)
        {
            targetNumber = AddZeroes(4);
        }
        else if (this.fileObjectNumber < ValueLevel3)
        {
            targetNumber = AddZeroes(3);
        }
        else if (this.fileObjectNumber < ValueLevel2)
        {
            targetNumber = AddZeroes(2);
        }
        else if (this.fileObjectNumber < ValueLevel1)
        {
            targetNumber = AddZeroes(1);
        }
        else
        {
            targetNumber = string.Empty;
        }

        return targetNumber + this.fileObjectNumber.ToString();
    }

    private static string AddZeroes(int countZeroes)
    {
        string zeroString = string.Empty;

        for (int i = 0; i < countZeroes; i++)
        {
            zeroString += AddZero();
        }

        return zeroString;
    }

    private static string AddZero() => "0";
}
