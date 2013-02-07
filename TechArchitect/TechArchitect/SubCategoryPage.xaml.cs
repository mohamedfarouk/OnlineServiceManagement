using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechArchitect.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace TechArchitect
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class SubCategoryPage : TechArchitect.Common.LayoutAwarePage
    {
        public SubCategoryPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            string navParam = (String)navigationParameter;
            string[] data = navParam.Split(new char[] { ',' });
            switch (data[0])
            {
                case "Category":
                    var category = SampleDataSource.GetCategory(data[1]);
                    this.DefaultViewModel["Category"] = category;
                    this.DefaultViewModel["SubCategories"] = category.SubCategories;
                    break;
                case "SubCategory":
                    var subcategory = SampleDataSource.GetSubCategory(data[1]);
                    this.DefaultViewModel["Category"] = subcategory;
                    this.DefaultViewModel["SubCategories"] = subcategory.SubCategories;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {

        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //if (e.ClickedItem.GetType() == typeof(SubCategory))
            //{
            //    var subCategoryID = ((SubCategory)e.ClickedItem).UniqueId;
            //    this.Frame.Navigate(typeof(SubCategoryPage), "SubCategory," + subCategoryID);
            //}
            //else
            //{
            //    var subCategoryID = ((Item)e.ClickedItem).UniqueId;
            //    this.Frame.Navigate(typeof(GroupDetailPage), subCategoryID);
            //}

            var subCategory = ((SubCategory)e.ClickedItem);
            if (subCategory.SubCategories.Count > 0)
            {
                this.Frame.Navigate(typeof(SubCategoryPage), "SubCategory," + subCategory.UniqueId);
            }
            else
            {
                this.Frame.Navigate(typeof(GroupDetailPage), subCategory.UniqueId);
            }
        }
    }
}
