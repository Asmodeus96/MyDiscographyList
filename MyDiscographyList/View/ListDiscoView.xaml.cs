using MyDiscographyList.Extention;
using MyDiscographyList.Model;
using MyDiscographyList.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyDiscographyList.View
{
    public partial class ListDiscoView : UserControl
    {
        
        public event EventHandler ArtistSelected;
        private bool isUserInteraction = false;

        public ListDiscoView()
        {
            InitializeComponent();
            dgArtistList.MouseDoubleClick += new MouseButtonEventHandler(GoToArtistDetail);
        }

        private void GoToArtistDetail(object sender, RoutedEventArgs e)
        {
            if (dgArtistList.SelectedItem == null) return;
            var selectedArtist = dgArtistList.SelectedItem as ArtistModel;
            ArtistRoutedEventArgs artistREA = new ArtistRoutedEventArgs(selectedArtist.ArtistId);
            ArtistSelected(this, artistREA);
        }

        private void UpdateStatusHandler(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded || !isUserInteraction) { return; }

            var comboBox = sender as ComboBox;
            var selectedItem = dgArtistList.CurrentItem as ArtistModel;
            var vm = DataContext as ArtistListViewModel;

            if (selectedItem != null && vm != null)
            {
                vm.UpdateSelectedArtistStatus();
            }

            isUserInteraction = false;
        }

        private void UpdateScoreHandler(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded || !isUserInteraction) { return; }

            var comboBox = sender as ComboBox;
            var selectedItem = dgArtistList.CurrentItem as ArtistModel;
            var vm = DataContext as ArtistListViewModel;

            if (selectedItem != null && vm != null)
            {
                vm.UpdateSelectedArtistScore();
            }

            isUserInteraction = false;
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgArtistList.CurrentItem as ArtistModel;

            if (selectedItem != null)
            {
                DeleteArtistConfirmView modalWindow = new DeleteArtistConfirmView(selectedItem);
                modalWindow.ArtistDeleted += new EventHandler(RemoveArtist);
                modalWindow.ShowDialog();
            }
        }

        private void RemoveArtist(Object sender, EventArgs e)
        {
            var selectedItem = dgArtistList.CurrentItem as ArtistModel;
            var vm = DataContext as ArtistListViewModel;

            if (selectedItem != null && vm != null)
            {
                vm.DeleteSelectedArtist();
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isUserInteraction = true;
        }
    }
}
