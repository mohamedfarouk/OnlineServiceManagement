using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechArchitect.Data
{
    public class CategoryItem : INotifyPropertyChanged
    {
        private string categoryItemID;

        public string CategoryItemID
        {
            get { return categoryItemID; }
            set { categoryItemID = value; OnPropertyChanged("CategoryItemID"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
