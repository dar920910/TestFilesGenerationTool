using System.Xml.Serialization;
using static System.Console;

namespace TestFilesGenerator.Library;

public class MediaCollection
{
    [XmlAttribute("Alias")]
    public string Alias { get; set; }

    [XmlAttribute("SourceMediaFile")]
    public string SourceMediaFile { get; set; }

    [XmlAttribute("CountOfIDs")]
    public uint CountOfObjects { get; set; }

    [XmlAttribute("RandomMode")]
    public bool IsRandom { get; set; }

    public MediaCollection() { }

    public MediaCollection(string alias, string source, uint count)
    {
        Alias = alias;
        SourceMediaFile = source;
        CountOfObjects = count;
        IsRandom = false;
    }

    public MediaCollection(string alias, string source, uint count, bool isRandom)
    {
        Alias = alias;
        SourceMediaFile = source;
        CountOfObjects = count;
        IsRandom = isRandom;
    }

    public void Out()
    {
        WriteLine($"\nMediaCollection: \"{Alias}\"\n");
        WriteLine($"-> SourceMediaFile: \"{SourceMediaFile}\"");
        WriteLine($"-> CountOfObjects: \"{CountOfObjects}\"");
        WriteLine($"-> IsRandom: \"{IsRandom}\"");
        WriteLine();
    }

    public List<MediaObject> RetrieveMediaObjects()
    {
        var mediaObjects = new List<MediaObject>();
        MediaObject[] arrayOfMediaObjects = GenerateMediaObjects();

        foreach (var mediaObject in arrayOfMediaObjects)
        {
            mediaObjects.Add(mediaObject);
        }

        return mediaObjects;
    }

    private MediaObject[] GenerateMediaObjects()
    {
        var arrayOfMediaObjects = new MediaObject[CountOfObjects];

        for (uint idCounter = 0; idCounter < arrayOfMediaObjects.Length; idCounter++)
        {
            string mediaName = IsRandom ? GenerateRandomIdName() : GenerateCustomIdName(idCounter + 1);
            string mediaPath = GenerateTargetIdPath(mediaName);
            arrayOfMediaObjects[idCounter] = new MediaObject(mediaPath, SourceMediaFile);
        }

        return arrayOfMediaObjects;
    }

    private string GenerateTargetIdPath(string targetIdName)
    {
        return ( GetOutputDirectory() + targetIdName );
    }

    private string GenerateCustomIdName(uint idNumber)
    {
        return ( Alias + "_" + GeneratorService.GetIdNumber(idNumber) + GetMediaFileExtensionFromSource() );
    }

    private string GenerateRandomIdName()
    {
        // Nexio ID can be consisting from 32 characters.
        // I set the rule that the name has 16 lower and 16 upper case letters.
        return ( (new RandomFileName(16, 16)).Generate() + GetMediaFileExtensionFromSource() );
    }

    private string GetMediaFileExtensionFromSource()
    {
        return (new FileInfo(SourceMediaFile)).Extension;
    }

    private string GetAliasByDefault() => "MEDIA";

    private string GetOutputDirectory()
    {
        string outputDirectory = $"{MediaStorage.OutputDirectory}{Alias}\\";

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return outputDirectory;
    }
}
