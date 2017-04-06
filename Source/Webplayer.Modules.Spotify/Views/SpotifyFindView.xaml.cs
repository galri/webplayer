using Infrastructure;
using Prism;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Webplayer.Modules.Spotify.ViewModels;
using Webplayer.Services.Spotify;

namespace Webplayer.Modules.Spotify.Views
{
    /// <summary>
    /// Interaction logic for SpotifyFindView.xaml
    /// </summary>
    public partial class SpotifyFindView : ISpoifyFindView
    {
        public string Title { get; set; } = "Spotify";

        //public Visibility SearchFieldVisibility
        //{
        //    get { return (Visibility)GetValue(SearchFieldVisibilityProperty); }
        //    set { SetValue(SearchFieldVisibilityProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for SearchFieldVisibility.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SearchFieldVisibilityProperty =
        //    DependencyProperty.Register("SearchFieldVisibility", typeof(Visibility), 
        //        typeof(SpotifyFindView), new PropertyMetadata(Visibility.Hidden,SearchFieldVisibilityChanged));

        //private static void SearchFieldVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var newValue = (Visibility)e.NewValue;
        //    if(newValue == Visibility.Visible)
        //    {
        //        var view = (SpotifyFindView)d;
        //        view.SearchField.Dispatcher.BeginInvoke(new Action(() => 
        //        {
        //            Keyboard.Focus(view.SearchField);

        //        }),System.Windows.Threading.DispatcherPriority.Render );
        //    }
        //}

        public SpotifyFindView()
        {
            InitializeComponent();
        }

        private void ListView_ScrollChanged(object sender, RoutedEventArgs e)
        {
            //endless loop
            //var sc = (ScrollViewer)e.OriginalSource;
            //if(sc.ContentVerticalOffset == sc.VerticalOffset)
            //{
            //    var vm = (ISpotifyFindViewModel)DataContext;
            //    if (vm.FetchMoreResultCommand.CanExecute(null))
            //    {
            //        vm.FetchMoreResultCommand.Execute(null);
            //    }
            //}
            //SearchFieldVisibility = Visibility.Hidden;
        }

        private void FindExecute(object sender, ExecutedRoutedEventArgs e)
        {
            //SearchFieldVisibility = Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBorder.Visibility = Visibility.Hidden;
        }
    }
}
