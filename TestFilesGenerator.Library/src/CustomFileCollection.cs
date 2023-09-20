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

    private string GenerateCustomIdName(uint idNumber)
    {
        return Alias + "_" + GeneratorService.GetIdNumber(idNumber) + GetFileExtensionFromSource();
    }

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
