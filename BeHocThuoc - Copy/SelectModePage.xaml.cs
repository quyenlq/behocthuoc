using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeHocThuoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectModePage : Page
    {
        public SelectModePage ()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
        }


        private void Long_Clicked (object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (LongMode));
        }

        private void Quick_Clicked (object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (QuickMode));
        }

        private void GoBack (object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (StartPage));
        }
    }
}