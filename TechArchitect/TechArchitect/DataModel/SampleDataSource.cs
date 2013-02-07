using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace TechArchitect.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : TechArchitect.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description, string backGroundColor)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = "Assets/Images/" + imagePath;
            this._backGroundColor = backGroundColor;            
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }

        private string _backGroundColor = string.Empty;
        public string BackGroundColor
        {
            get { return this._backGroundColor; }
            set { this.SetProperty(ref this._backGroundColor, value); }
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class Item : SampleDataCommon
    {
        public Item(String uniqueId, String title, String subtitle, String imagePath, String description, string backGroundColor, String content, SubCategory SubCategory)
            : base(uniqueId, title, subtitle, imagePath, description, backGroundColor)
        {
            this._content = content;
            this._subCategory = SubCategory;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

         private SubCategory _subCategory;
        public SubCategory SubCategory
        {
            get { return this._subCategory; }
            set { this.SetProperty(ref this._subCategory, value); }
        }
    }

    public class SubCategory : SampleDataCommon
    {
        public SubCategory(String uniqueId, String title, String subtitle, String imagePath, String description, string backGroundColor)
            : base(uniqueId, title, subtitle, imagePath, description, backGroundColor)
        {
            
        }

        private ObservableCollection<SubCategory> _subCategories = new ObservableCollection<SubCategory>();
        public ObservableCollection<SubCategory> SubCategories
        {
            get { return this._subCategories; }
        }

        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items
        {
            get { return this._items; }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class Category: SampleDataCommon
    {
        public Category(String uniqueId, String title, String subtitle, String imagePath, String description,string backGroundColor)
            : base(uniqueId, title, subtitle, imagePath, description, backGroundColor)
        {
            
        }

        private ObservableCollection<SubCategory> _subCategories = new ObservableCollection<SubCategory>();
        public ObservableCollection<SubCategory> SubCategories
        {
            get { return this._subCategories; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<Category> _allCategories = new ObservableCollection<Category>();
        public ObservableCollection<Category> AllCategories
        {
            get { return this._allCategories; }
        }

        public static IEnumerable<Category> GetGetCategories(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _sampleDataSource.AllCategories;
        }

        public static IEnumerable<SubCategory> GetSubCategories(string uniqueId)
        {
            var foundCategory = _sampleDataSource.AllCategories.FirstOrDefault(g => g.UniqueId == uniqueId);
            if (foundCategory != null)
            {
                return foundCategory.SubCategories;
            }
            else
            {
                return null;
            }
        }

        public static Category GetCategory(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllCategories.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }


        public static SubCategory GetSubCategory(ObservableCollection<SubCategory> SubCategories, string uniqueId)
        {
            SubCategory foundSubCategory = SubCategories.Where(s => s.UniqueId == uniqueId).FirstOrDefault();
            if (foundSubCategory == null)
            {
                foreach (SubCategory sCategory in SubCategories)
                {
                    foundSubCategory = GetSubCategory(sCategory.SubCategories, uniqueId);
                    if (foundSubCategory != null)
                    {
                        break;
                    }
                }
            }
            return foundSubCategory;
        }


        public static SubCategory GetSubCategory(string uniqueId)
        {
            SubCategory foundSubCategory = null;
            foreach (Category category in _sampleDataSource.AllCategories)
            {
                foundSubCategory = GetSubCategory(category.SubCategories, uniqueId);
                if (foundSubCategory != null)
                {
                    break;
                }
            }
            return foundSubCategory;
        }

        //public static SampleDataItem GetItem(string uniqueId)
        //{
        //    // Simple linear search is acceptable for small data sets
        //    var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
        //    if (matches.Count() == 1) return matches.First();
        //    return null;
        //}

        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var GetToKnow = new Category("GetToKnow",
                    "Get to Know",
                    "Learning",
                    "GettoKnow.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante",
                    "Green");

            #region Microsoft Technologies           

            var MicTechnology = new SubCategory("MicTechnology",
                    "Microsoft Technologies",
                    "Microsoft Corporation",
                    "microsoft.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante",
                    "Red");

            SubCategory WinApp = new SubCategory("WinApp",
                    "Windows Applications",
                    "Desktop or Client application",
                    "microsoft/winforms.png",
                    "Windows-based applications that run locally on users' computers.",
                    "Blue");

            WinApp.Items.Add(new Item("WinForms",
                    "Windows Forms",
                    "Windows Forms provides a cross-platform way to design graphical user interfaces",
                    "microsoft/winforms.png",
                    "Windows Forms is mainly a wrapper around the Windows API, and some of the methods allow direct access to Win32 callbacks, which are not available in non-Windows platforms.\r\n C#, VB.NET",
                    "Blue",
                    ITEM_CONTENT,
                    WinApp));

            WinApp.Items.Add(new Item("WPF",
                   "Windows Presentation Foundation",
                   "Presentation framework that can be used to develop Standalone Applications",
                   "microsoft/wpf.png",
                   "Windows Presentation Foundation (WPF) provides developers with a unified programming model for building rich Windows smart client user experiences that incorporate UI, media, and documents. \r\n XAML",
                   "Blue",
                   ITEM_CONTENT,
                   WinApp));

            MicTechnology.SubCategories.Add(WinApp);

            SubCategory WebApp = new SubCategory("WebApp",
                    "Web Appplications",
                    "Browser based Applications",
                    "microsoft/webforms.png",
                    "Web Forms are similar to Windows Forms in that they provide properties, methods, and events for the controls that are placed onto them. However, these UI elements render themselves in the appropriate markup language required by the request. e.g. HTML.",
                    "DarkGoldenrod");

            WebApp.Items.Add(new Item("WebForms",
                "ASP.NET",
                "Websites or Web applications",
                "microsoft/webforms.png",
                "A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access. \r\n C#, VB.NET",
                "DarkGoldenrod",
                ITEM_CONTENT,
                WebApp));

            WebApp.Items.Add(new Item("Silverlight",
                 "Silverlight",
                 "Rich Internet applications",
                 "microsoft/silverlight.png",
                 "Silverlight is a powerful development tool for creating engaging, interactive user experiences for Web and mobile applications. \r\n XAML",
                 "DarkGoldenrod",
                 ITEM_CONTENT,
                 WebApp));

            MicTechnology.SubCategories.Add(WebApp);

            SubCategory WinRT = new SubCategory("WinRT",
                    "Windows RT Appplications",
                    "",
                    "microsoft/winrt.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante",
                    "Crimson");

            MicTechnology.SubCategories.Add(WinRT);

            SubCategory Silverlight = new SubCategory("Silverlight",
                   "Silverlight Appplications",
                   "",
                   "microsoft/silverlight.png",
                   "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante",
                   "DarkViolet");

            MicTechnology.SubCategories.Add(Silverlight);

            GetToKnow.SubCategories.Add(MicTechnology);

            #endregion Microsoft Technologies

            #region Sun Microsystems

            var SunTechnology = new SubCategory("SunTechnology",
                    "Java Platform Enterprise Edition",
                    "Sun Microsystems",
                    "sunmicrosys.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante",
                    "Red");

            SubCategory JavaWinApp = new SubCategory("JavaWinApp",
                    "Windows Applications",
                    "Desktop or Client application",
                    "sunmicrosys/java.png",
                    "Windows-based applications that run locally on users' computers.",
                    "Blue");

            JavaWinApp.Items.Add(new Item("Swing",
                    "Java Swing",
                    "Java GUI widget toolkit",
                    "sunmicrosys/swing.png",
                    "Swing is a platform-independent, Model-View-Controller GUI framework for Java.\r\n Java",
                    "Blue",
                    ITEM_CONTENT,
                    JavaWinApp));

            SunTechnology.SubCategories.Add(JavaWinApp);

            SubCategory JavaWebApp = new SubCategory("JavaWebApp",
                    "Web Appplications",
                    "Browser based Applications",
                    "sunmicrosys/java.png",
                    "Web Forms are similar to Windows Forms in that they provide properties, methods, and events for the controls that are placed onto them. However, these UI elements render themselves in the appropriate markup language required by the request. e.g. HTML.",
                    "DarkGoldenrod");

            JavaWebApp.Items.Add(new Item("JSP",
                   "Java Server Page",
                   "JSP may be viewed as a high-level abstraction of Java servlets",
                   "sunmicrosys/jsp.png",
                   "JavaServer Pages (JSP) is a technology that helps software developers create dynamically generated web pages based on HTML, XML, or other document types.",
                   "DarkGoldenrod",
                   ITEM_CONTENT,
                   JavaWebApp));

            SunTechnology.SubCategories.Add(JavaWebApp);


            GetToKnow.SubCategories.Add(SunTechnology);

            #endregion Sun Microsystems

            this.AllCategories.Add(GetToKnow);
        }
    }
}
