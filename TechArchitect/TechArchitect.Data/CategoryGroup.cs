using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechArchitect.Data
{
    public class CategoryGroup : INotifyPropertyChanged
    {
        private string categoryGroupID;

        public string CategoryGroupID
        {
            get { return categoryGroupID; }
            set { categoryGroupID = value; OnPropertyChanged("CategoryGroupID"); }
        }

        private string categoryGroupName;

        public string CategoryGroupName
        {
            get { return categoryGroupName; }
            set { categoryGroupName = value; OnPropertyChanged("CategoryGroupName"); }
        }

        private List<CategoryGroup> categoryGroups;

        public List<CategoryGroup> CategoryGroups
        {
            get { return categoryGroups; }
            set { categoryGroups = value; OnPropertyChanged("CategoryGroups"); }
        }

        private List<CategoryItem> categoryItems;

        public List<CategoryItem> CategoryItems
        {
            get { return categoryItems; }
            set { categoryItems = value; OnPropertyChanged("CategoryItems"); }
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
