namespace TestFilesGenerator.Library;

public class MediabaseService
{
    private readonly MediaBase TargetMediaBase;
    public MediabaseService(MediaBase mediabase)
    {
        TargetMediaBase = mediabase;
    }

    public void CloneIDs()
    {
        CreateAllMedia(RetrieveAllMedia(new MediaBase()));
    }

    public void EraseIDs()
    {
        MediaStorage.CleanBusyDiskSpace();
    }

    private List<MediaObject> RetrieveAllMedia(MediaBase mediabase)
    {
        var allMedia = new List<MediaObject>();

        foreach (var mediaCollection in mediabase.MediaCollections)
        {
            List<MediaObject> mediaObjects = mediaCollection.RetrieveMediaObjects();

            foreach (var mediaObject in mediaObjects)
            {
                allMedia.Add(mediaObject);
            }
        }

        return allMedia;
    }

    private void CreateAllMedia(List<MediaObject> mediaObjects)
    {
        foreach (var mediaObject in mediaObjects)
        {
            mediaObject.Clone();
        }
    }
}
