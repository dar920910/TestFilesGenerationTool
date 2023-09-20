using System.Xml.Serialization;

namespace TestFilesGenerator.Library;

public class CustomFileStorage
{
    private readonly string configuration;
    public List<CustomFileCollection> UserFileCollections { get; }

    public CustomFileStorage()
    {
        configuration = GetConfigFilePathByDefault();
        UserFileCollections = RetrieveFileCollections();
    }

    private List<CustomFileCollection> RetrieveFileCollections()
    {
        var collection = new List<CustomFileCollection>();
        var serializer = new XmlSerializer(typeof(List<CustomFileCollection>));
    
        using (FileStream stream = File.Open(configuration, FileMode.Open))
        {
            collection = (List<CustomFileCollection>)serializer.Deserialize(stream);
        }

        return collection;
    }

    private static string GetConfigFilePathByDefault()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "config.xml");
    }
}
