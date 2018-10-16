using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ExemploMonkeyCache
{
    public partial class RSSWebViewPage : ContentPage
    {
        public RSSWebViewPage(string url)
        {
            InitializeComponent();

            webView.Source = url;
        }
    }
}
