namespace ProductivityTools.PhotoGallery.PhotoProcessing
{
    public class PhotoProcessingService
    {
        public void ConvertImage(string path, string thumbnailPath, int targetSize)
        {
            var image=NetVips.Image.NewFromFile(path);
            NetVips.Image thumbnail = image.ThumbnailImage(targetSize);
            thumbnail.WriteToFile(thumbnailPath);
        }
    }
}