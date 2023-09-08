using System.Xml.Serialization;

namespace TestFilesGenerator.Library;

public class MediaBase
{
    private readonly string mediabaseConfiguration;
    public List<MediaCollection> MediaCollections { get; }

    public MediaBase()
    {
        mediabaseConfiguration = GetMediaBaseConfigByDefault();
        MediaCollections = RetrieveListOfMediaCollections();
    }

    public MediaBase(string mediabaseConfigPath)
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

    private string GetMediaBaseConfigByDefault() => @".\mediabase.xml";
}
