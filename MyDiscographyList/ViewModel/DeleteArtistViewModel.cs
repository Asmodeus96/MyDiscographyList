using DataAccessLibrary;
using MyDiscographyList.Command;
using MyDiscographyList.Model;
using System;
using System.Windows.Input;

namespace MyDiscographyList.ViewModel
{
    class DeleteArtistViewModel
    {
        private ArtistModel _artist;
        private ICommand _deleteArtistCommand;
        private readonly bool canExecute = true;

        public DeleteArtistViewModel(ArtistModel artist)
        {
            Artist = artist;
            DeleteArtistCommand = new RelayCommand(DeleteArtist, param => this.canExecute);
        }

        public ArtistModel Artist { get { return _artist; } set { _artist = value; } }

        public Action CloseAction { get; set; }

        public ICommand DeleteArtistCommand { get { return _deleteArtistCommand; } set { _deleteArtistCommand = value; } }

        public void DeleteArtist(Object obj)
        {
            DataAccess.DeleteArtist(_artist); CloseAction();
            CloseAction();
        }
    }
}
