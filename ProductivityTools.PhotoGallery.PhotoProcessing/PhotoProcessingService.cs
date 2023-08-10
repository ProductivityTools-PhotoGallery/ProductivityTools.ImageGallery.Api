namespace ProductivityTools.PhotoGallery.PhotoProcessing
{
    public class PhotoProcessingService
    {
        public void ConvertImage(string path, string thumbnailPath)
        {
            var image=NetVips.Image.NewFromFile(path);
            NetVips.Image thumbnail = image.ThumbnailImage(300);
            thumbnail.WriteToFile(thumbnailPath);
        }
    }
}