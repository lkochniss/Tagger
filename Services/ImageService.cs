using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;
using Tagger.Modells;

namespace Tagger.Services
{
    internal class ImageService
    {
        private ImageProperties properties;
        private List<int> taggedImages;

        public async Task<TagImage> ShowRandomImage(StorageFolder imageFolder, string tag)
        {
            CommonFileQuery query = CommonFileQuery.DefaultQuery;
            var queryOptions = new QueryOptions(query, new[] {".jpg", ".jpeg" })
            {
                FolderDepth = FolderDepth.Deep
            };
            _ = imageFolder.CreateFileQueryWithOptions(queryOptions);

            var images = await imageFolder.GetFilesAsync();

            TagImage image = await GenerateImage(images, tag);
            while (image.Tags.Contains(tag) && taggedImages.Count < images.Count)
            {
                if (taggedImages.Contains(image.GetHashCode()) == false) {
                    taggedImages.Add(image.GetHashCode());
                }
                image = await GenerateImage(images, tag);
            }

            return image;
        }

        public async void SaveImageTag(string tag)
        {
            properties.Keywords.Add(tag);
            await properties.SavePropertiesAsync();
        }

        private async Task<TagImage> GenerateImage(IReadOnlyList<StorageFile> images, string tag)
        {
            var random = new Random();
            var source = images[random.Next(images.Count)];

            using (var stream = await source.OpenAsync(FileAccessMode.ReadWrite))
            {
                var previewImage = new BitmapImage();
                await previewImage.SetSourceAsync(stream);
                var tags = await GetTags(source);

                return new TagImage { ImageData = previewImage, Tags = tags };
            }
        }

        private async Task<List<string>> GetTags(StorageFile image)
        {
            properties = await image.Properties.GetImagePropertiesAsync();

            List<string> tags = new List<string>();

            foreach (string keyword in properties.Keywords)
            {
                tags.Add(keyword);
            }

            return tags;
        }
    }
}
