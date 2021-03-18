using MyDiscographyList.Model;
using MyDiscographyList.ViewModel;
using System;
using System.Windows;

namespace MyDiscographyList.View
{
    public partial class DeleteArtistConfirmView : Window
    {
        public event EventHandler ArtistDeleted;

        public DeleteArtistConfirmView(ArtistModel artist)
        {
            InitializeComponent();
            DeleteArtistViewModel vm = new DeleteArtistViewModel(artist);
            DataContext = vm;
            vm.CloseAction = new Action(this.Close);
        }

        private void DeleteArtist(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs artistREA = new RoutedEventArgs();
            ArtistDeleted(this, artistREA);
        }
    }
}
