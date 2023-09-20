using System.Xml.Serialization;

namespace TestFilesGenerator.Library;

public class CustomFileStorage
{
    private readonly string mediabaseConfiguration;
    public List<MediaCollection> MediaCollections { get; }

    public CustomFileStorage()
    {
        mediabaseConfiguration = GetMediaBaseConfigByDefault();
        MediaCollections = RetrieveListOfMediaCollections();
    }

    private List<MediaCollection> RetrieveListOfMediaCollections()
    {
        var media = new List<MediaCollection>();
        var mediabaseSerializer = new XmlSerializer(typeof(List<MediaCollection>));
    
        using (FileStream mediabaseXmlConfig = File.Open(mediabaseConfiguration, FileMode.Open))
        {
            media = (List<MediaCollection>)mediabaseSerializer.Deserialize(mediabaseXmlConfig);
        }

        return media;
    }

    private string GetMediaBaseConfigByDefault()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "config.xml");
    }
}
