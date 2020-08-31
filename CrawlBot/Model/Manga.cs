using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlBot.Model
{
    public class Manga : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private IList<int> user_id;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public IList<int> User_id
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
                OnPropertyChanged("User_id");
            }
        }


        #region INotifyPropertyChanged  
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
