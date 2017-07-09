using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpriteSheeter
{
    class SelectFilesCommand : ICommand
    {
        public delegate void FilesSelectedEventHandler(object sender, FilesSelectedEventArgs args);

        public class FilesSelectedEventArgs : EventArgs
        {
            private string[] _selectedFiles;

            public string[] SelectedFiles
            {
                get
                {
                    string[] selectedFiles = new string[_selectedFiles.Length];
                    Array.Copy(_selectedFiles, selectedFiles, _selectedFiles.Length);
                    return selectedFiles;
                }
            }

            public FilesSelectedEventArgs(string[] selectedFiles)
            {
                _selectedFiles = new string[selectedFiles.Length];
                Array.Copy(selectedFiles, _selectedFiles, selectedFiles.Length);
            }
        }

        protected string _filter;
        protected bool _multiSelect;

        public event EventHandler CanExecuteChanged;
        public event FilesSelectedEventHandler FilesSelected;

        public SelectFilesCommand(string filter, bool multiSelect)
        {
            _filter = filter;
            _multiSelect = multiSelect;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _filter;
            openFileDialog.Multiselect = _multiSelect;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                var handler = FilesSelected;
                handler?.Invoke(this, new FilesSelectedEventArgs(openFileDialog.FileNames));
            }
        }
    }
}
