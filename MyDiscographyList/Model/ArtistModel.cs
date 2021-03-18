using System;
using System.ComponentModel;

namespace MyDiscographyList.Model
{
    public class ArtistModel : INotifyPropertyChanged
    {
        private int artistId;
        private string artistName;
        private string artistAlias;
        private ArtistScoreModel artistScore;
        private ArtistStatusModel artistStatus;
        private bool artisUpToDate;
        private DateTime artistDate;
       

        public int ArtistId
        {
            get { return artistId; }
            set { artistId = value; OnPropertyChanged("ArtistId"); }
        }

        public string ArtistName
        {
            get { return artistName; }
            set { artistName = value; OnPropertyChanged("ArtistName"); }
        }

        public string ArtistAlias
        {
            get { return artistAlias; }
            set { artistAlias = value; OnPropertyChanged("ArtistAlias"); }
        }

        public ArtistScoreModel ArtistScore
        {
            get { return artistScore; }
            set { artistScore = value; OnPropertyChanged("ArtistScore"); }
        }

        public ArtistStatusModel ArtistStatus
        {
            get { return artistStatus; }
            set { artistStatus = value; OnPropertyChanged("ArtistStatus"); }
        }

        public bool ArtistUpToDate
        {
            get { return artisUpToDate; }
            set { artisUpToDate = value; OnPropertyChanged("ArtistUpToDate"); }
        }

        public DateTime ArtistDate
        {
            get { return artistDate; }
            set { artistDate = value; OnPropertyChanged("ArtistUpToDate"); }
        }


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
