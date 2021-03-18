using DataAccessLibrary;
using MyDiscographyList.Command;
using MyDiscographyList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyDiscographyList.ViewModel
{
    public class AddArtistViewModel
    {
        private ArtistModel _artist;
        private List<ArtistStatusModel> _artistStatusList;
        private List<ArtistScoreModel> _artistScoreList;
        private ICommand _addArtistCommand;
        private readonly bool canExecute = true;

        public AddArtistViewModel()
        {
            Artist = new ArtistModel();
            ArtistStatuses = DataAccess.GetStatusList();
            ArtistScores = DataAccess.GetScoreList();
            Artist.ArtistStatus = ArtistStatuses[5];
            Artist.ArtistScore = ArtistScores[0];
            Artist.ArtistUpToDate = false;
            Artist.ArtistAlias = "";

            AddArtistCommand = new RelayCommand(AddArtist, param => this.canExecute);
        }

        public ArtistModel Artist { get { return _artist; } set { _artist = value; } }

        public List<ArtistStatusModel> ArtistStatuses { get { return _artistStatusList; } set { _artistStatusList = value; } }

        public List<ArtistScoreModel> ArtistScores { get { return _artistScoreList; } set { _artistScoreList = value; } }

        public Action CloseAction { get; set; }

        public ICommand AddArtistCommand { get {  return _addArtistCommand; } set { _addArtistCommand = value; } }

        public void AddArtist(Object obj) { DataAccess.AddArtist(_artist); CloseAction(); }
    }
}
