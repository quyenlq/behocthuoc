using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BeHocThuoc.Common;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace BeHocThuoc.Data
{
    /// <summary>
    /// Base class for <see cref="Card"/> and <see cref="Category"/> that
    /// defines properties common to both.
    /// </summary>
    [WebHostHidden]
    public abstract class CardCommon : BindableBase 
    {
        public static Uri _baseUri = new Uri("ms-appx:///");

        private ImageSource _back;
        private ImageSource _front;
        private ImageSource _display;
        private String _imagePath1;
        private String _imagePath2;
        private string _uniqueId = string.Empty;

        public CardCommon(String uniqueId, String imagePath1, String imagePath2)
        {
            _uniqueId = uniqueId;
            _imagePath1 = imagePath1;
            _imagePath2 = imagePath2;
        }

        public String UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        public ImageSource DisplayImage {
            get { return _display; }
            set { SetProperty(ref _display, value); }
        }

        public ImageSource FrontImage
        {
            get
            {
                if (_front == null && _imagePath1 != null)
                {
                    _front = new BitmapImage(new Uri(_baseUri, _imagePath1));
                }
                return _front;
            }

            set
            {
                _imagePath1 = null;
                SetProperty(ref _front, value);
            }
        }

        public ImageSource BackImage
        {
            get
            {
                if (_back == null && _imagePath2 != null)
                {
                    _back = new BitmapImage(new Uri(_baseUri, _imagePath2));
                }
                return _back;
            }

            set
            {
                _imagePath1 = null;
                SetProperty(ref _back, value);
            }
        }

        public void SetFontImage(String path)
        {
            _front = null;
            _imagePath1 = path;
            OnPropertyChanged("FrontImage");
        }

        public void SetBackImage(String path)
        {
            _back = null;
            _imagePath2 = path;
            OnPropertyChanged("BackImage");
        }

        public override string ToString()
        {
            return UniqueId;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class Card : CardCommon
    {
        private Category _group;

        public Card(String uniqueId, String imagePath1, String imagePath2, Category group)
            : base(uniqueId, imagePath1, imagePath2)
        {
            _group = group;
        }

        public Category Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class Category : BindableBase
    {
        private readonly ObservableCollection<Card> _items = new ObservableCollection<Card>();
        private readonly ObservableCollection<Card> _topItem = new ObservableCollection<Card>();
        private ImageSource _image;
        private string _imagePath;
        private string _title;
        private String _uniqueId = string.Empty;

        public Category(String uniqueId, String title, String imagePath)

        {
            _uniqueId = uniqueId;
            _title = title;
            _imagePath = imagePath;
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        public string UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ImageSource FrontImage
        {
            get
            {
                if (_image == null && _imagePath != null)
                {
                    _image = new BitmapImage(new Uri(CardCommon._baseUri, _imagePath));
                }
                return _image;
            }

            set
            {
                _imagePath = null;
                SetProperty(ref _image, value);
            }
        }

        public ObservableCollection<Card> Items
        {
            get { return _items; }
        }

        public ObservableCollection<Card> TopItems
        {
            get { return _topItem; }
        }

        public Card LastItem
        {
            get { return Items.Last(); }
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
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
        private static readonly SampleDataSource _sampleDataSource = new SampleDataSource();

        private readonly ObservableCollection<Card> _allCards = new ObservableCollection<Card>();
        private readonly ObservableCollection<Category> _allGroups = new ObservableCollection<Category>();

        public SampleDataSource()
        {
            var group1 = new Category("Động Vật",
                                      "Động Vật",
                                      "Assets/Animal/animal.jpg");
            group1.Items.Add(new Card("Động-Vật-Item-1",
                                      "Assets/Animal/b_01.jpg",
                                      "Assets/Animal/f_01.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-2",
                                      "Assets/Animal/b_02.jpg",
                                      "Assets/Animal/f_02.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-3",
                                      "Assets/Animal/b_03.jpg",
                                      "Assets/Animal/f_03.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-4",
                                      "Assets/Animal/b_04.jpg",
                                      "Assets/Animal/f_04.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-5",
                                      "Assets/Animal/b_05.jpg",
                                      "Assets/Animal/f_05.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-6",
                                      "Assets/Animal/b_06.jpg",
                                      "Assets/Animal/f_06.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-7",
                                      "Assets/Animal/b_07.jpg",
                                      "Assets/Animal/f_07.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-8",
                                      "Assets/Animal/b_08.jpg",
                                      "Assets/Animal/f_08.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-9",
                                      "Assets/Animal/b_09.jpg",
                                      "Assets/Animal/f_09.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-10",
                                      "Assets/Animal/b_10.jpg",
                                      "Assets/Animal/f_10.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-11",
                                      "Assets/Animal/b_11.jpg",
                                      "Assets/Animal/f_11.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-12",
                                      "Assets/Animal/b_12.jpg",
                                      "Assets/Animal/f_12.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-13",
                                      "Assets/Animal/b_13.jpg",
                                      "Assets/Animal/f_13.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-14",
                                      "Assets/Animal/b_14.jpg",
                                      "Assets/Animal/f_14.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-15",
                                      "Assets/Animal/b_15.jpg",
                                      "Assets/Animal/f_15.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-16",
                                      "Assets/Animal/b_16.jpg",
                                      "Assets/Animal/f_16.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            group1.Items.Add(new Card("Động-Vật-Item-17",
                                      "Assets/Animal/b_17.jpg",
                                      "Assets/Animal/f_17.jpg",
                                      group1));
            AllCards.Add(group1.LastItem);
            AllGroups.Add(group1);

            var group2 = new Category("Đồ đạc trong nhà",
                                      "Đồ đạc trong nhà",
                                      "Assets/Funiture/funiture.jpg");
            group2.Items.Add(new Card("Đồ-đạc-Item-1",
                                      "Assets/Funiture/b_01.jpg",
                                      "Assets/Funiture/f_01.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-2",
                                      "Assets/Funiture/b_02.jpg",
                                      "Assets/Funiture/f_02.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-3",
                                      "Assets/Funiture/b_03.jpg",
                                      "Assets/Funiture/f_03.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-4",
                                      "Assets/Funiture/b_04.jpg",
                                      "Assets/Funiture/f_04.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-5",
                                      "Assets/Funiture/b_05.jpg",
                                      "Assets/Funiture/f_05.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-6",
                                      "Assets/Funiture/b_06.jpg",
                                      "Assets/Funiture/f_06.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-7",
                                      "Assets/Funiture/b_07.jpg",
                                      "Assets/Funiture/f_07.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-8",
                                      "Assets/Funiture/b_08.jpg",
                                      "Assets/Funiture/f_08.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-9",
                                      "Assets/Funiture/b_09.jpg",
                                      "Assets/Funiture/f_09.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            group2.Items.Add(new Card("Đồ-đạc-Item-10",
                                      "Assets/Funiture/b_10.jpg",
                                      "Assets/Funiture/f_10.jpg",
                                      group2));
            AllCards.Add(group2.LastItem);
            AllGroups.Add(group2);
        }

        public ObservableCollection<Category> AllGroups
        {
            get { return _allGroups; }
        }

        public ObservableCollection<Card> AllCards
        {
            get { return _allCards; }
        }

        public static IEnumerable<Category> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups"))
                throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _sampleDataSource.AllGroups;
        }

        public static ObservableCollection<Card> GetCards()
        {
            return _sampleDataSource.AllCards;
        }

        public static Category GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            IEnumerable<Category> matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static Category GetGroup(int index)
        {
            // Simple linear search is acceptable for small data sets
            var groups = _sampleDataSource.AllGroups[index];
            return groups;
        }

        public static Card GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            IEnumerable<Card> matches =
                _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where(
                    (item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
    }
}