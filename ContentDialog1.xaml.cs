using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Tagger
{
    public sealed partial class ContentDialog1 : ContentDialog
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ContentDialog1), new PropertyMetadata(default(string)));

        public ContentDialog1()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void CancelClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void SaveClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
