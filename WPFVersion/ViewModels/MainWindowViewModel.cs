using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using QRCoder;
using Xamarin.Forms;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace WPFVersion.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ConvertCommand = new Command(async () => Convert());
        }

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        private string _inputText = String.Empty;

        public string QrImagePath
        {
            get=> _qrImagePath;
            set
            {
                _qrImagePath = value;
                OnPropertyChanged(nameof(QrImagePath));
            }
        }

        private string _qrImagePath = $"{Directory.GetCurrentDirectory()}\\Images\\DefaultQr.png";

        public string LogoPath
        {
            get => _logoPath;
            set
            {
                _logoPath = value;
                OnPropertyChanged(nameof(LogoPath));
            }
        }

        private string _logoPath = null;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        private string _fileName = "CustomQR";

        public ICommand ConvertCommand { get; private set; }


        public async void Convert()
        {
            QRCodeGenerator qrCodeGenerator = new();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(_inputText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.Title = "Save QR File";
            saveFileDialog.FileName = _fileName;

            if (saveFileDialog.ShowDialog() == true)
            {
                Stream myStream;
                if ((myStream = saveFileDialog.OpenFile()) is { } stream)
                {
                    qrCodeImage.Save(myStream, System.Drawing.Imaging.ImageFormat.Png);
                    myStream.Close();
                    QrImagePath = saveFileDialog.FileName;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}