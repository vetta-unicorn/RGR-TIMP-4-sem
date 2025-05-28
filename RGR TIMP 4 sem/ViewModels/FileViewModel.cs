using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace RGR_TIMP_4_sem.ViewModels
{
    public class FileItem
    {
        public string FileName { get; set; }
    }

    public class FileList : ReactiveObject
    {
        private ObservableCollection<FileItem> _fileItems;
        public ObservableCollection<FileItem> FileItems
        {
            get => _fileItems;
            set => this.RaiseAndSetIfChanged(ref _fileItems, value);
        }

        public FileList(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                var fileNames = Directory.GetFiles(directoryPath);
                FileItems = new ObservableCollection<FileItem>(
                    fileNames.Select(fileName => new FileItem { FileName = Path.GetFileName(fileName) }));
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
            }
        }

        public void RefreshFileList(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                var fileNames = Directory.GetFiles(directoryPath);
                FileItems = new ObservableCollection<FileItem>(
                    fileNames.Select(fileName => new FileItem { FileName = Path.GetFileName(fileName) }));
            }
        }
    }

}
