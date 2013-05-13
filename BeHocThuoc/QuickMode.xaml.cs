using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BeHocThuoc.Common;
using BeHocThuoc.Data;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeHocThuoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuickMode : LayoutAwarePage
    {
        private const int DURATION = 30;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private DateTime EndTime;
        private MessageDialog messageDialog1;
        public ObservableCollection<Card> randomCards = new ObservableCollection<Card>();

        public QuickMode()
        {
            InitializeComponent();
            GameInitialize();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        /// 
        /// 
        /// 
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }


        private void GameInitialize()
        {
            TimesUpPopup.IsOpen = false;
            GenerateRandomCards();
            PlayGround.DataContext = randomCards;
            Start_Timer(DURATION);
        }


        private void GenerateRandomCards()
        {
            if (randomCards != null)
                randomCards.Clear();

            var rnd = new Random();

            // random group index
            int groupIndex = rnd.Next(0, SampleDataSource.GetGroups("AllGroups").Count());

            //shuffle cards list
            var rCards = SampleDataSource.GetGroup(groupIndex).Items.OrderBy(x => Guid.NewGuid()).ToArray();
            //get first 4 cards
            for (int i = 0; i < 4; i++)
            {
                rCards[i].DisplayImage = rCards[i].FrontImage;
                randomCards.Add(rCards[i]);
            }
        }

        private void Start_Timer(int seconds)
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            EndTime = DateTime.Now.AddSeconds(seconds);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            TimeSpan timeleft = EndTime - DateTime.Now;
            TimeField.Text = ((int) timeleft.TotalSeconds).ToString();
            if ((int) timeleft.TotalSeconds < 6)
            {
                TimeField.Foreground = new SolidColorBrush(Colors.Orange);
            }
            if ((int) timeleft.TotalSeconds <= 3)
            {
                TimeField.Foreground = new SolidColorBrush(Colors.Red);
            }
            if ((int) timeleft.TotalSeconds <= 0)
            {
                timer.Stop();
                TimesUpPopup.IsOpen = true;
            }
        }


        private void Toogle_Card(object sender, TappedRoutedEventArgs e)
        {
            var card = PlayGround.SelectedItem as Card;
            if (card == null || TimesUpPopup.IsOpen==true) return;
            if (card.DisplayImage == card.BackImage)
            {
                card.DisplayImage = card.FrontImage;
            }
            else card.DisplayImage = card.BackImage;
        }


        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (timer != null) timer.Stop();
            base.GoBack(sender, e);
        }

        private void OK_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), randomCards);
        }

        private void NotYet_Clicked(object sender, RoutedEventArgs e)
        {
            GameInitialize();
        }
    }
}