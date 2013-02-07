using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TechArchitect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingSplashScreen : Page
    {
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        private SplashScreen splash;
        internal Frame rootFrame;

        public LoadingSplashScreen(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();
            lblProgress.Text = "Loading ....";
            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);
            }
            rootFrame = new Frame();
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }

        DispatcherTimer dispatcherTimer;
        int timesTicked = 1;

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
            lblProgress.Text = "Connecting to database ....";
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            timesTicked++;
            if (timesTicked == 1)
            {
                lblProgress.Text = "Populating data ....";
            }

            if (timesTicked == 2)
            {
                lblProgress.Text = "Initialisation Completed";
                dispatcherTimer.Stop();
                //Frame rootFrame = Window.Current.Content as Frame;
                // Set extended splash info on main page
                //((MainPage)rootFrame.Content).SetExtendedSplashInfo(splashImageRect, dismissed);
                rootFrame.Navigate(typeof(CategoryPage), "AllGroups");
                // Place the frame in the current Window 
                Window.Current.Content = rootFrame;
            }
        }

        void LoadingSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimerSetup();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
    }
}
