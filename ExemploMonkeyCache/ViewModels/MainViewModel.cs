using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Parsers.Rss;
using ExemploMonkeyCache.Models;
using Xamarin.Forms;
using MonkeyCache.SQLite;

namespace ExemploMonkeyCache.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Property

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        private string _key = "rssFeed"; //Chave com o nome do objeto que armazena os dados.


        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<RssData> RSSFeed { get; }

        public MainViewModel()
        {
            RSSFeed = new ObservableCollection<RssData>();
           
            CarregaRSS();
           
        }

      
        private ICommand _refreshNewsFeedCommand;

        public ICommand RefreshNewsFeedCommand =>
            _refreshNewsFeedCommand ?? (_refreshNewsFeedCommand = new Command(
                async () =>
                {
                   await GravarRSS();
                }));


        private async Task GravarRSS()
        {
            IsBusy = true;
            string feed = null;

            using (var client = new HttpClient())
            {

                feed = await client.GetStringAsync("https://medium.com/feed/@bertuzzi");

            }

            if (feed != null)
            {
                RSSFeed.Clear();

                var existingList = Barrel.Current.Get<List<RssData>>(_key) ?? new List<RssData>();

                var parser = new RssParser();
                var rss = parser.Parse(feed).OrderByDescending(e => e.PublishDate).ToList(); ;


                foreach (var rssSchema in rss)
                {
                    var isExist = existingList.Any(e => e.Guid == rssSchema.InternalID);

                    var rssdata = new RssData
                    {
                        Title = rssSchema.Title,
                        PubDate = rssSchema.PublishDate,
                        Link = rssSchema.FeedUrl,
                        Guid = rssSchema.InternalID,
                        Author = rssSchema.Author,
                        Thumbnail = string.IsNullOrWhiteSpace(rssSchema.ImageUrl) ? $"https://placeimg.com/80/80/nature" : rssSchema.ImageUrl,
                        Description = rssSchema.Summary
                    };

                    if (!isExist)
                    {
                        existingList.Add(rssdata);
                    }

                    RSSFeed.Add(rssdata);
                }

                existingList = existingList.OrderByDescending(e => e.PubDate).ToList();

                Barrel.Current.Add(_key, existingList, TimeSpan.FromDays(30));
            }
            IsBusy = false;
        }


        private async Task CarregaRSS()
        {
            IsBusy = true;
            RSSFeed.Clear();

            var existingList = Barrel.Current.Get<List<RssData>>(_key) ?? new List<RssData>();

            if (existingList.Count == 0)
                await GravarRSS();
            else
            {
                var newsFeed = Barrel.Current.Get<List<RssData>>(_key);

                foreach (var rssData in newsFeed)
                {
                    RSSFeed.Add(rssData);
                }

            }
            IsBusy = false;
        }

    }
}

