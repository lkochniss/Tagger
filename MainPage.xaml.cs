using Tagger.Modells;
using Tagger.Services;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace Tagger
{
    public sealed partial class MainPage : Page
    {
        public MainPage()   
        {
            InitializeComponent();
            imageService = new ImageService();
        }

        private string tag = "Witcher 3";
        private StorageFolder folder;
        private TagImage currentImage;
        private readonly ImageService imageService;

        private void UpdateInfoBox()
        {
            var localTag = "Select Tag";
            if (tag != null)
            {
                localTag = tag;
            }

            var localDisplayName = "Select Directory";
            if (folder != null)
            {
                localDisplayName = folder.DisplayName;
            }

            InfoBox.Text = "Directory=" + localDisplayName + " | Tag=" + localTag;
        }


        private async void SyncfolderClicked(object sender, RoutedEventArgs e)
        {
            SyncDirectoryService syncDirectoryService = new SyncDirectoryService();
            StorageFolder folder = await syncDirectoryService.OpenPicker();
            this.folder = folder;

            UpdateInfoBox();
            GetNextImage();
        }

        private void NopeClicked(object sender, RoutedEventArgs e)
        {
            if (currentImage == null) return;
            GetNextImage();
        }

        private void YesClicked(object sender, RoutedEventArgs e)
        {
            if (currentImage == null) return;
            imageService.SaveImageTag(tag);
            GetNextImage();
        }

        private async void GetNextImage()
        {
            if (tag == null || folder == null) return;

            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            var image = await imageService.ShowRandomImage(folder, tag);
            if (image != null)
            {
                currentImage = image;
                ImageDetails.Source = image.ImageData;
                Tags.ItemsSource = image.Tags;
            }
        }


        private async void TagClicked(object sender, RoutedEventArgs e)
        {
            var dialog1 = new ContentDialog1();
            var result = await dialog1.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                tag = dialog1.Text;
            }

            UpdateInfoBox();
        }
    }
}
