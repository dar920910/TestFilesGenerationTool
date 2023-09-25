using System.Xml.Serialization;
using static System.Console;

namespace TestFilesGenerator.Library;

public class CustomFileCollection
{
    [XmlAttribute("Alias")]
    public string Alias { get; set; }

    [XmlAttribute("SourceFileObject")]
    public string SourceFileObject { get; set; }

    [XmlAttribute("CountOfIDs")]
    public uint CountOfObjects { get; set; }

    [XmlAttribute("RandomMode")]
    public bool IsRandom { get; set; }

    public CustomFileCollection() { }

    public CustomFileCollection(string alias, string source, uint count)
    {
        Alias = alias;
        SourceFileObject = source;
        CountOfObjects = count;
        IsRandom = false;
    }

    public CustomFileCollection(string alias, string source, uint count, bool isRandom)
    {
        Alias = alias;
        SourceFileObject = source;
        CountOfObjects = count;
        IsRandom = isRandom;
    }

    public void Out()
    {
        WriteLine($"\nCustomFileCollection: \"{Alias}\"\n");
        WriteLine($"-> SourceFileObject: \"{SourceFileObject}\"");
        WriteLine($"-> CountOfObjects: \"{CountOfObjects}\"");
        WriteLine($"-> IsRandom: \"{IsRandom}\"");
        WriteLine();
    }

    public List<CustomFileObject> RetrieveFileObjects()
    {
        var fileObjects = new List<CustomFileObject>();
        CustomFileObject[] arrayOfFileObjects = GenerateFileObjects();

        foreach (var fileObject in arrayOfFileObjects)
        {
            fileObjects.Add(fileObject);
        }

        return fileObjects;
    }

    private CustomFileObject[] GenerateFileObjects()
    {
        var arrayOfFileObjects = new CustomFileObject[CountOfObjects];

        for (uint idCounter = 0; idCounter < arrayOfFileObjects.Length; idCounter++)
        {
            string fileName = IsRandom ? GenerateRandomIdName() : GenerateCustomIdName(idCounter + 1);
            string filePath = GenerateTargetIdPath(fileName);
            arrayOfFileObjects[idCounter] = new CustomFileObject(filePath, SourceFileObject);
        }

        return arrayOfFileObjects;
    }

    private string GenerateTargetIdPath(string targetIdName)
    {
        return Path.Combine(GetOutputDirectory(), targetIdName);
    }

    private string GenerateCustomIdName(uint idNumber) =>
        $"{this.Alias}_{new CollectionFileObjectNumberId(idNumber).Create()}{this.GetFileExtensionFromSource()}";

    private string GenerateRandomIdName()
    {
        // Nexio ID can be consisting from 32 characters.
        // I set the rule that the name has 16 lower and 16 upper case letters.
        return new RandomFileName(16, 16).Generate() + GetFileExtensionFromSource();
    }

    private string GetFileExtensionFromSource()
    {
        return new FileInfo(SourceFileObject).Extension;
    }

    private string GetOutputDirectory()
    {
        string outputDirectory = Path.Combine(FileDriveManager.OutputDirectory, Alias);

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return outputDirectory;
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