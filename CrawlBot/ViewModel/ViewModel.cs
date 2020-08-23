using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrawlBot.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool crawling = false;
        public bool Crawling
        {
            get { return crawling; }
            set
            {
                crawling = value;
                OnPropertyChanged("Crawling");
            }
        }

        public Command.Command Tgl_crawling_command { get; private set; }




        public ViewModel()
        {
            Tgl_crawling_command = new Command.Command(Tgl_crawling);
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        public void Tgl_crawling()
        {
            Crawling = !Crawling;
        }
    }
}
