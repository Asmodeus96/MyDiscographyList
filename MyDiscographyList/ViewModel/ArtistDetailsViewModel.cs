using DataAccessLibrary;
using MyDiscographyList.Command;
using MyDiscographyList.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using SpotifyAPI.Web;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyDiscographyList.ViewModel
{
    class ArtistDetailsViewModel : INotifyPropertyChanged
    {
        private ArtistModel _artist;
        private ArtistModel _artistSelected;
        private ObservableCollection<ArtistModel> _artistsRecommandationList;
        private ObservableCollection<ArtistModel> _artistsRelatedList;
        private List<ArtistStatusModel> _artistStatusList;
        private List<ArtistScoreModel> _artistScoreList;
        private string _deezerId;
        private string _spotifyId;

        private bool _showRelatedArtitList;
        private bool _noRelatedToAdd;

        private ICommand _updateArtistUpToDateCommand;
        private ICommand _changeArtistNameCommand;
        private ICommand _changeArtistAliasCommand;
        private ICommand _getDeezerRecommandationCommand;
        private ICommand _getSpotifyRecommandationCommand;
        private ICommand _insertSelectedArtistCommand;
        private readonly bool canExecute = true;

        public ArtistDetailsViewModel(int id)
        {
            Artist = DataAccess.GetArtistById(id);
            ArtistStatuses = DataAccess.GetStatusList();
            ArtistScores = DataAccess.GetScoreList();
            ArtistSelected = new ArtistModel();
            ArtistsRecommandationList = DataAccess.GetRelatedArtist(Artist.ArtistId);
            ArtistsRelatedList = new ObservableCollection<ArtistModel>();

            ShowRelatedArtistList = false;
            NoRelatedToAdd = false;

            UpdateArtistUpToDateCommand = new RelayCommand(UpdateArtistUpToDate, param => this.canExecute);
            ChangeArtistNameCommand = new RelayCommand(ChangeArtistName, param => this.canExecute);
            ChangeArtistAliasCommand = new RelayCommand(ChangeArtistAlias, param => this.canExecute);
            GetDeezerRecommandationCommand = new RelayCommand(GetDeezerRecommandation, param => this.canExecute);
            GetSpotifyRecommandationCommand = new RelayCommand(GetSpotifyRecommandation, param => this.canExecute);
            InsertSelectedArtistCommand = new RelayCommand(InsertSelectedArtist, param => this.canExecute);
        }


        public ArtistModel Artist { get { return _artist; } set { _artist = value; } }

        public ObservableCollection<ArtistModel> ArtistsRecommandationList { get { return _artistsRecommandationList; } set { _artistsRecommandationList = value; } }

        public ObservableCollection<ArtistModel> ArtistsRelatedList { get { return _artistsRelatedList; } set { _artistsRelatedList = value; } }

        public ArtistModel ArtistSelected { get { return _artistSelected; } set { _artistSelected = value; } }

        public List<ArtistStatusModel> ArtistStatuses { get { return _artistStatusList; } set { _artistStatusList = value; } }

        public List<ArtistScoreModel> ArtistScores { get { return _artistScoreList; } set { _artistScoreList = value; } }

        public string DeezerId { get { return _deezerId; } set { _deezerId = value; } }

        public string SpotifyId { get { return _spotifyId; } set { _spotifyId = value; } }

        public bool ShowRelatedArtistList { get { return _showRelatedArtitList; } set { _showRelatedArtitList = value; OnPropertyChanged("ShowRelatedArtistList"); } }

        public bool NoRelatedToAdd { get { return _noRelatedToAdd; } set { _noRelatedToAdd = value; OnPropertyChanged("NoRelatedToAdd"); } }


        public ICommand UpdateArtistUpToDateCommand { get { return _updateArtistUpToDateCommand; } set { _updateArtistUpToDateCommand = value; } }

        public ICommand ChangeArtistNameCommand { get { return _changeArtistNameCommand; } set { _changeArtistNameCommand = value; } }

        public ICommand ChangeArtistAliasCommand { get { return _changeArtistAliasCommand; } set { _changeArtistAliasCommand = value; } }

        public ICommand GetDeezerRecommandationCommand { get { return _getDeezerRecommandationCommand; } set { _getDeezerRecommandationCommand = value; } }

        public ICommand GetSpotifyRecommandationCommand { get { return _getSpotifyRecommandationCommand; } set { _getSpotifyRecommandationCommand = value; } }

        public ICommand InsertSelectedArtistCommand { get { return _insertSelectedArtistCommand; } set { _insertSelectedArtistCommand = value; } }


        public void ChangeArtistName(Object obj) { DataAccess.UpdateArtistName(_artist); }

        public void ChangeArtistAlias(Object obj) { DataAccess.UpdateArtistAlias(_artist); }

        public void UpdateArtistUpToDate(Object obj) { DataAccess.UpdateArtistUpToDate(_artist); }

        public void UpdatedArtistStatus() { DataAccess.UpdateArtistStatus(_artist); }

        public void UpdateArtistScore() { DataAccess.UpdateArtistScore(_artist); }

        public async void GetDeezerRecommandation(Object obj)
        {
            if (!string.IsNullOrEmpty(DeezerId))
            {
                List<string> nameList = new List<string>();

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.deezer.com/artist/" + DeezerId + "/related"))
                    {
                        request.Headers.TryAddWithoutValidation("Accept", "application/json");
                        var response = await httpClient.SendAsync(request);
                        var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                        foreach (var name in json["data"])
                        {
                            nameList.Add(name["name"].ToString());
                        }
                    }
                }

                UpdateRelatedList(nameList);
            }
        }

        public async void GetSpotifyRecommandation(Object obj)
        {
            if (!string.IsNullOrEmpty(SpotifyId))
            {
                List<string> nameList = new List<string>();
                var config = SpotifyClientConfig.CreateDefault();
                var request = new ClientCredentialsRequest("349e633cba5b499183d80b075c90da6c", "a19042332bd5470f9fc9d2fca33c7b23");
                var response = await new OAuthClient(config).RequestToken(request);
                var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
                var related = await spotify.Artists.GetRelatedArtists(SpotifyId);

                foreach (var name in related.Artists)
                {
                    nameList.Add(name.Name.ToString());
                }

                UpdateRelatedList(nameList);
            }
        }

        public void UpdateRelatedList(List<string> nameList)
        {
            foreach (var artist in ArtistsRecommandationList)
            {
                nameList.Remove(artist.ArtistName);
                nameList.Remove(artist.ArtistAlias);
            }

            if (nameList.Count > 0)
            {
                var relatedArtistRegistred = DataAccess.CheckRelatedArtists(nameList);

                foreach (var artist in relatedArtistRegistred)
                {
                    ArtistsRelatedList.Add(artist);
                }

                foreach (var name in ArtistsRelatedList)
                {
                    nameList.Remove(name.ArtistName);
                    nameList.Remove(name.ArtistAlias);
                }
            }

            foreach (var name in nameList)
            {
                ArtistsRelatedList.Add(new ArtistModel() {ArtistId = -1, ArtistName = name, ArtistScore = ArtistScores[0], ArtistStatus = ArtistStatuses[5], ArtistAlias = "", ArtistUpToDate = false });
            }

            if (ArtistsRelatedList.Count > 0)
            {
                ShowRelatedArtistList = true;
                NoRelatedToAdd = false;
            }
            else
            {
                ShowRelatedArtistList = false;
                NoRelatedToAdd = true;
            }
        }

        public void UpdateSelectedArtistStatus() { DataAccess.UpdateArtistStatus(_artistSelected); }

        public void UpdateSelectedArtistScore() { DataAccess.UpdateArtistScore(_artistSelected); }

        public void InsertSelectedArtist(Object obj)
        {
            int insertedId;

            if (ArtistSelected.ArtistId < 0)
            {
                insertedId = DataAccess.AddArtist(_artistSelected);
                _artistSelected.ArtistId = insertedId;
            }
            else
            {
                insertedId = ArtistSelected.ArtistId;
            }

            DataAccess.AddRecommandation(Artist.ArtistId, insertedId);
            ArtistsRelatedList.Remove(_artistSelected);

            if (ArtistsRelatedList.Count > 0)
            {
                ShowRelatedArtistList = true;
                NoRelatedToAdd = false;
            }
            else
            {
                ShowRelatedArtistList = false;
                NoRelatedToAdd = true;
            }

            ArtistsRecommandationList.Add(DataAccess.GetArtistById(insertedId));
        }

        public void DeleteSelectedArtist() { ArtistsRecommandationList.Remove(ArtistSelected); }


        #region INotifyPropertyChanged Artist  

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
