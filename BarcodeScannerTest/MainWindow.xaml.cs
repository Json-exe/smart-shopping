using System.Drawing;
using System.Windows;
using Emgu.CV;
using Microsoft.Win32;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace BarcodeScannerTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void UploadImage_OnClick(object sender, RoutedEventArgs e)
    {
        var filePicker = new OpenFileDialog
        {
            Title = "Pick an  image file"
        };
        if (filePicker.ShowDialog() != true) return;
        using var image = Image.FromStream(filePicker.OpenFile());
        using var bitmap = new Bitmap(image);
        var reader = new BarcodeReader
        {
            Options = new DecodingOptions
            {
                TryHarder = true
            },
            AutoRotate = true
        };
        var result = reader.Decode(bitmap);
        if (result is not null)
        {
            BarcodeText.Text = result.Text;
            BarcodeTextType.Text = result.BarcodeFormat.ToString();
        }
        else
        {
            BarcodeText.Text = "Failed to read data!";
            BarcodeTextType.Text = "Unknown";
        }
    }
}