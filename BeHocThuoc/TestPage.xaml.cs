using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BeHocThuoc.Common;
using BeHocThuoc.Data;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BeHocThuoc
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TestPage : LayoutAwarePage
    {
        private int duration = 60;
        private int result = 0;
        private int questions = 0;
        private int totalquestion=0;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private DateTime EndTime;
        private Card answerCard;
        private ObservableCollection<Card>aCard = new ObservableCollection<Card>();
        private ObservableCollection<Card> questionCards= new ObservableCollection<Card>();
        private ObservableCollection<Card> randomCards;
        private IOrderedEnumerable<Card> qCards;

        public TestPage()
        {
            InitializeComponent();
            Start_Timer(duration);
        }

        public void GameInitialize()
        {
            GetAnswerCard(randomCards);
            GetQuestionCards(randomCards);
            QuestionGround.DataContext = qCards;
            AnswerGround.DataContext = aCard;
            btnHint.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            randomCards = e.Parameter as ObservableCollection<Card>;
            var tmp = randomCards.Count;
            if (tmp == 4) totalquestion = 2;
            else totalquestion = 4;
            Result.Text = "Traû lôøi ñuùng: " + result + "/" + totalquestion;
            GameInitialize();
        }

        private void Start_Timer(int duration)
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            EndTime = DateTime.Now.AddSeconds(duration);
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
            if ((int)timeleft.TotalSeconds <= 0)
            {
                timer.Stop();
                resultTxt.Text = result + "/" + totalquestion;
                CorrectPopup.IsOpen = false;
                IncorrectPopup.IsOpen = false;
                FinishPopup.IsOpen = true;

            }

        }

        private void GetAnswerCard(ObservableCollection<Card> randomCards)
        {
            var rnd = new Random();
            int index = rnd.Next(0, totalquestion-1);
            if (answerCard == null)
                answerCard = randomCards[index];
            else
            {
                randomCards.Remove(answerCard);
                var temp = randomCards.OrderBy(x => Guid.NewGuid()).ToArray();
                answerCard = temp[0];
            }
            aCard.Clear();
            aCard.Add(answerCard.Clone());
            aCard[0].DisplayImage = aCard[0].FrontImage;
        }



        private void GetQuestionCards(ObservableCollection<Card> randomCards)
        {
            questionCards.Clear();
            Category category = randomCards.First().Group;
            ObservableCollection<Card> cards = category.Items;

            //Remove the answer card to generate non-duplicate question card
            var tmp = cards.First(x => x.UniqueId == answerCard.UniqueId);
            cards.Remove(tmp);
            //Shuffle the all cards list in the category
            var cards_list = cards.OrderBy(x => Guid.NewGuid()).ToArray();
            for (int i = 0; i < 3; i++)
            {
                cards_list[i].DisplayImage = cards_list[i].BackImage;
                questionCards.Add(cards_list[i]);
            }
            answerCard.DisplayImage = answerCard.BackImage;
            questionCards.Add(answerCard);
            
            qCards = questionCards.OrderBy(x => Guid.NewGuid());
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

        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (timer != null) timer.Stop();
            base.GoBack(sender, e);
        }


        private void Check_Correct(object sender, TappedRoutedEventArgs e)
        {
            aCard[0].DisplayImage = aCard[0].BackImage;
            var card = QuestionGround.SelectedItem as Card;
            if(card.UniqueId == answerCard.UniqueId)
            {
                result++;
                questions++;
                Result.Text = "Traû lôøi ñuùng: " + result + "/"+totalquestion;
                CorrectPopup.IsOpen = true;
                IncorrectPopup.IsOpen = false;
            }
            else
            {
                questions++;
                IncorrectPopup.IsOpen = true;
                CorrectPopup.IsOpen = false;
            }
        }


        private void NextBtn_Clicked(object sender, RoutedEventArgs e)
        {
            
            CorrectPopup.IsOpen = false;
            IncorrectPopup.IsOpen = false;
            if (questions < totalquestion)
                GameInitialize();
            else
            {
                FinishPopup.IsOpen = true;
                resultTxt.Text = result + "/" + totalquestion;
            }
        }

        private void Return_Clicked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.Frame.Navigate(typeof(StartPage), "AllGroups");
        }

        private void Replay_Clicked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if(totalquestion!=4){
                this.Frame.Navigate(typeof (QuickMode), "AllGroups");
            }
            else
            {
                this.Frame.Navigate(typeof (LongMode), "AllGroupds");
            }
        }


        private void Hint_Clicked(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            var tmp1 = qCards.ToList();
            var tmp2 = qCards.ToList();
            tmp1.Remove(answerCard);
            tmp1=tmp1.OrderBy(x => Guid.NewGuid()).ToList();
            for (int j = 0; j < 2; j++)
            {
                tmp2.Remove(tmp1[j]);
            }
            QuestionGround.DataContext = tmp2;
            btnHint.Visibility = Visibility.Collapsed;
            
        }
    }
}