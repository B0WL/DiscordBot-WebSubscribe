using CrawlBot.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrawlBot.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private bool crawling = false;
        private IList<Manga> manga_list;
        private void Tgl_crawling() =>
            Crawling = !Crawling;

        public bool Crawling
        {
            get { return crawling; }
            set
            {
                crawling = value;
                OnPropertyChanged("Crawling");
            }
        }
        public IList<Manga> Manga_list
        {
            get { return manga_list; }
            set {
                manga_list = value;
                OnPropertyChanged("Manga");
            }
        }
        public Command.Command Tgl_crawling_command { get; private set; }


        public MainWindowVM()
        {
            Tgl_crawling_command = new Command.Command(Tgl_crawling);
        }






        #region INotifyPropertyChanged  
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
