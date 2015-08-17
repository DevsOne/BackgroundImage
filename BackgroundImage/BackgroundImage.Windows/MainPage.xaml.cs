using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BackgroundImage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public static BitmapImage BitmapImage = new BitmapImage();
        private async void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IRandomAccessStream fileStream;
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.ViewMode = PickerViewMode.Thumbnail;

            //Filter to include a sample subset of file types
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".tiff");

            //Open the file picker
            StorageFile file = await openPicker.PickSingleFileAsync();

            //file is null if user cancels the file picker.
            if (file != null)
            {
                fileStream = await file.OpenAsync(FileAccessMode.Read);

                BitmapImage.SetSource(fileStream);
                displayImage.ImageSource = BitmapImage;
                this.DataContext = file;

                await file.CopyAsync(ApplicationData.Current.LocalFolder, "background.png", NameCollisionOption.ReplaceExisting);
            }

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            IStorageItem fileCheck1 = await folder.TryGetItemAsync("background.png");

            if (fileCheck1 != null)
            {
                StorageFile file = await folder.GetFileAsync("background.png");
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage.SetSource(fileStream);
                displayImage.ImageSource = BitmapImage;
            }
            else
            {

                //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/red.png"));
                //IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                //BitmapImage.SetSource(fileStream);
                //displayImage.ImageSource = BitmapImage;
                displayImage.ImageSource = null;

            }

        }
    }
}
