using System.Xml.Serialization;

namespace TestFilesGenerator.Library;

public class CustomFileStorage
{
    private readonly string mediabaseConfiguration;
    public List<CustomFileCollection> MediaCollections { get; }

    public CustomFileStorage()
    {
        mediabaseConfiguration = GetMediaBaseConfigByDefault();
        MediaCollections = RetrieveListOfMediaCollections();
    }

    private List<CustomFileCollection> RetrieveListOfMediaCollections()
    {
        var media = new List<CustomFileCollection>();
        var mediabaseSerializer = new XmlSerializer(typeof(List<CustomFileCollection>));
    
        using (FileStream mediabaseXmlConfig = File.Open(mediabaseConfiguration, FileMode.Open))
        {
            media = (List<CustomFileCollection>)mediabaseSerializer.Deserialize(mediabaseXmlConfig);
        }

        return media;
    }

    private string GetMediaBaseConfigByDefault()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "config.xml");
    }
}
