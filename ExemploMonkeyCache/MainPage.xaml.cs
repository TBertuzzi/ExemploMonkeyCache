using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExemploMonkeyCache.Models;
using ExemploMonkeyCache.ViewModels;
using Xamarin.Forms;

namespace ExemploMonkeyCache
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }

        void ListView_OnItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var selectedItem = (RssData)e.Item;
            Navigation.PushAsync(new RSSWebViewPage(selectedItem.Link));
        }
    }
}
