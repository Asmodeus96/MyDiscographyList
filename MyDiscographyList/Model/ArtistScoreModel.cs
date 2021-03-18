using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiscographyList.Model 
{
    public class ArtistScoreModel : INotifyPropertyChanged
    {
        private int _scoreId;
        private string _scoreLabel;

        public int ScoreId { get { return _scoreId; } set { _scoreId = value; OnPropertyChanged("ScoreId"); } }

        public string ScoreLabel { get { return _scoreLabel; } set { _scoreLabel = value; OnPropertyChanged("ScoreLabel"); } }


        #region INotifyPropertyChanged Artis  

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
