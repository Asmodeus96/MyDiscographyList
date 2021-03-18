using MyDiscographyList.Extention;
using MyDiscographyList.Model;
using MyDiscographyList.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyDiscographyList.View
{
    public partial class ArtistDetailsView : UserControl
    {
        public event EventHandler ArtistSelected;
        private bool isUserInteraction = false;

        public ArtistDetailsView()
        {
            InitializeComponent();
            dgArtistListRecommandation.MouseDoubleClick += new MouseButtonEventHandler(GoToArtistDetail);
        }

        private void UpdateScoreHandler(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded || !isUserInteraction) { return; }

            ArtistModel selectedItem = dgArtistListRecommandation.CurrentItem as ArtistModel;
            var vm = DataContext as ArtistDetailsViewModel;

            if (selectedItem != null && vm != null)
            {
                vm.UpdateSelectedArtistScore();
            }
            else if (vm != null)
            {
                vm.UpdateArtistScore();
            }

            isUserInteraction = false;
        }

        private void UpdateStatusHandler(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded || !isUserInteraction) { return; }

            var selectedItem = dgArtistListRecommandation.CurrentItem as ArtistModel;
            var vm = DataContext as ArtistDetailsViewModel;

            if (selectedItem != null && vm != null)
            {
                vm.UpdateSelectedArtistStatus();
            }
            else if (vm != null)
            {
                vm.UpdatedArtistStatus();
            }

            isUserInteraction = false;
        }

        private void GoToArtistDetail(object sender, RoutedEventArgs e)
        {
            if (dgArtistListRecommandation.SelectedItem == null) return;
            var selectedArtist = dgArtistListRecommandation.SelectedItem as ArtistModel;
            ArtistRoutedEventArgs artistREA = new ArtistRoutedEventArgs(selectedArtist.ArtistId);
            ArtistSelected(this, artistREA);
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgArtistListRecommandation.CurrentItem as ArtistModel;

            if (selectedItem != null)
            {
                DeleteArtistConfirmView modalWindow = new DeleteArtistConfirmView(selectedItem);
                modalWindow.ArtistDeleted += new EventHandler(RemoveArtist);
                modalWindow.ShowDialog();
            }
        }

        private void RemoveArtist(Object sender, EventArgs e)
        {
            var selectedItem = dgArtistListRecommandation.CurrentItem as ArtistModel;
            var vm = DataContext as ArtistDetailsViewModel;

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