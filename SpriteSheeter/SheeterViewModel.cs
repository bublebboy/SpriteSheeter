using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinMedia = System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace SpriteSheeter
{
    class SheeterViewModel : INotifyPropertyChanged
    {
        private ICommand _selectImagesCommand;
        private ICommand _saveImageCommand;
        private WinMedia.Brush _preview = null;
        private double _previewWidth = 100;
        private double _previewHeight = 100;

        public event PropertyChangedEventHandler PropertyChanged;

        public SheeterViewModel()
        {
            SelectFilesCommand selectFilesCommand = new SelectFilesCommand("Image Files (*.png)|*.png", true);
            selectFilesCommand.FilesSelected += OnFilesSelected;
            _selectImagesCommand = selectFilesCommand;
        }

        public ICommand SelectImagesCommand
        {
            get { return _selectImagesCommand; }
        }

        public WinMedia.Brush Preview
        {
            get { return _preview; }
            private set
            {
                _preview = value;
                OnPropertyChanged("Preview");
            }
        }

        public double PreviewWidth
        {
            get { return _previewWidth; }
            private set
            {
                _previewWidth = value;
                OnPropertyChanged("PreviewWidth");
            }
        }

        public double PreviewHeight
        {
            get { return _previewHeight; }
            private set
            {
                _previewHeight = value;
                OnPropertyChanged("PreviewHeight");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void OnFilesSelected(object sender, SelectFilesCommand.FilesSelectedEventArgs args)
        {
            string[] selectedFiles = args.SelectedFiles;
            if (selectedFiles.Length == 0)
            {
                return;
            }

            // Assume all images are the same size
            int imagesPerSide = (int)Math.Ceiling(Math.Log(selectedFiles.Length, 2));

            Image image = Image.FromFile(selectedFiles[0]);
            int inImageDimensions = Math.Max(image.Width, image.Height);
            int finalImageDimensions = inImageDimensions * imagesPerSide;

            using (Bitmap destinationBitmap = new Bitmap(finalImageDimensions, finalImageDimensions))
            {
                using (Graphics graphics = Graphics.FromImage(destinationBitmap))
                {
                    for (int i = 0; i < selectedFiles.Length; i++)
                    {
                        string path = selectedFiles[i];

                        if (i > 0)
                        {
                            image = Image.FromFile(selectedFiles[i]);
                        }

                        int x = (i % imagesPerSide) * inImageDimensions;
                        int y = (int)Math.Floor((float)i / imagesPerSide) * inImageDimensions;
                        Console.WriteLine("Point( {0}, {1} )", x, y);

                        graphics.DrawImage(image, x, y, inImageDimensions, inImageDimensions);

                        image.Dispose();
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image(*.png)|*.png";

                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    destinationBitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
                    Preview = new WinMedia.ImageBrush(new BitmapImage(new Uri(saveFileDialog.FileName, UriKind.Absolute)));
                    PreviewWidth = finalImageDimensions;
                    PreviewHeight = finalImageDimensions;
                }
            }
        }
    }
}
