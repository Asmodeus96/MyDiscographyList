using MyDiscographyList.Enum;
using MyDiscographyList.Extention;
using MyDiscographyList.View;
using MyDiscographyList.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyDiscographyList
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleBtnUnchecked(object sender, RoutedEventArgs e) { Bg.Opacity = 1; }
        private void ToggleBtnChecked(object sender, RoutedEventArgs e) { Bg.Opacity = 0.3; }
        private void CloseBtnClick(object sender, RoutedEventArgs e) { Close(); }

        private void AddBtnClick(object sender, RoutedEventArgs e)
        {
            AddArtistModalWindowView modalWindow = new AddArtistModalWindowView();
            modalWindow.ShowDialog();
        }

        private void GoToHome(object sender, RoutedEventArgs e)
        {
            MainView.Items.Clear();
            var userControl = new HomeView();
            DataContext = new DashboardViewModel();
            MainView.Items.Add(new TabItem { Content = userControl });
            MainView.Items.Refresh();
        }

        private void GoToListenedDisco(object sender, RoutedEventArgs e)
        {
            MainView.Items.Clear();
            var userControl = new ListDiscoView();
            DataContext = new ArtistListViewModel(TypeListEnum.Listened);
            userControl.ArtistSelected += new EventHandler(GoToArtistDetails);
            MainView.Items.Add(new TabItem { Content = userControl });
            MainView.Items.Refresh();
        }

        private void GoToUnlistenedDisco(object sender, RoutedEventArgs e)
        {
            MainView.Items.Clear();
            var userControl = new ListDiscoView();
            DataContext = new ArtistListViewModel(TypeListEnum.Unlistened);
            userControl.ArtistSelected += new EventHandler(GoToArtistDetails);
            MainView.Items.Add(new TabItem { Content = userControl });
            MainView.Items.Refresh();
        }

        private void GoToSearchedDisco(object sender, RoutedEventArgs e)
        {
            MainView.Items.Clear();
            var userControl = new ListDiscoView();
            DataContext = new ArtistListViewModel(TypeListEnum.Searched, searchTxt.Text);
            userControl.ArtistSelected += new EventHandler(GoToArtistDetails);
            MainView.Items.Add(new TabItem { Content = userControl });
            MainView.Items.Refresh();
        }

        private void GoToArtistDetails(Object sender, EventArgs e)
        {
            ArtistRoutedEventArgs artist = (ArtistRoutedEventArgs)e;
            MainView.Items.Clear();
            var userControl = new ArtistDetailsView();
            DataContext = new ArtistDetailsViewModel(artist.ArtistID);
            userControl.ArtistSelected += new EventHandler(GoToArtistDetails);
            MainView.Items.Add(new TabItem { Content = userControl });
            MainView.Items.Refresh();
        }

        private void GoToGithub(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Asmodeus96/MyDiscographyList");
        }
    }
}
