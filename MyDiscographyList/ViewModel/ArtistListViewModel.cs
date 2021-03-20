using DataAccessLibrary;
using MyDiscographyList.Command;
using MyDiscographyList.Enum;
using MyDiscographyList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MyDiscographyList.ViewModel
{
    class ArtistListViewModel
    {
        private ObservableCollection<ArtistModel> _artistList;
        private List<ArtistStatusModel> _artistStatusList;
        private List<ArtistScoreModel> _artistScoreList;
        private List<string> _firstLetterFilterList;
        private ArtistModel _artistSelected;
        private ICommand _updateArtistUpToDateCommand;
        private ICommand _firstLetterFilterCommand;
        private readonly bool canExecute = true;
        private TypeListEnum _typeList;

        public ArtistListViewModel(TypeListEnum type, string filter = "All")
        {
            ArtistStatuses = DataAccess.GetStatusList();
            ArtistScores = DataAccess.GetScoreList();
            TypeList = type;
            ArtistList = DataAccess.GetArtistList(TypeList, filter);

            FirstLetterFilterList = Enumerable.Range('A', 26).Select(x => (char) x).Select(c => c.ToString()).ToList();
            FirstLetterFilterList.Insert(0, "All");
            FirstLetterFilterList.Add("0-9");
            FirstLetterFilterList.Add("*");

            ArtistSelected = new ArtistModel();
            UpdateArtistUpToDateCommand = new RelayCommand(UpdateArtistUpToDate, param => canExecute);
            FirstLetterFilterCommand = new RelayCommand(FirstLetterFilter, param => canExecute);
        }

        public ObservableCollection<ArtistModel> ArtistList { get { return _artistList; } set { _artistList = value; }}

        public List<ArtistStatusModel> ArtistStatuses { get { return _artistStatusList; } set { _artistStatusList = value; }}

        public List<ArtistScoreModel> ArtistScores { get { return _artistScoreList; } set { _artistScoreList = value; }}

        public List<string> FirstLetterFilterList { get { return _firstLetterFilterList; } set { _firstLetterFilterList = value; } }

        public ArtistModel ArtistSelected { get { return _artistSelected; } set { _artistSelected = value; }}

        public TypeListEnum TypeList { get { return _typeList; } set { _typeList = value; } }

        public ICommand UpdateArtistUpToDateCommand { get { return _updateArtistUpToDateCommand; } set { _updateArtistUpToDateCommand = value; }}
        public ICommand FirstLetterFilterCommand { get { return _firstLetterFilterCommand; } set { _firstLetterFilterCommand = value; }}


        public void UpdateArtistUpToDate(Object obj) { DataAccess.UpdateArtistUpToDate(_artistSelected); }

        public void UpdateSelectedArtistStatus() 
        { 
            DataAccess.UpdateArtistStatus(ArtistSelected); 


            switch (TypeList)
            {
                case TypeListEnum.Listened:
                    if (_artistSelected.ArtistStatus.StatusId == 6)
                    {
                        ArtistList.Remove(_artistSelected);
                    }
                    break;
                case TypeListEnum.Unlistened:
                    
                    if (_artistSelected.ArtistStatus.StatusId == 6)
                    {
                        ArtistList.Add(_artistSelected);
                    }
                    ArtistList.Remove(_artistSelected);
                    break;
            }
        }

        public void UpdateSelectedArtistScore() { DataAccess.UpdateArtistScore(ArtistSelected); }

        public void DeleteSelectedArtist() { ArtistList.Remove(ArtistSelected); }

        public void FirstLetterFilter(Object obj) 
        {
            ArtistList.Clear();
            ObservableCollection<ArtistModel> listTmp = DataAccess.GetArtistList(TypeList, obj.ToString());
            foreach (var tmp in listTmp)
            {
                ArtistList.Add(tmp);
            } 
        }
    }
}
