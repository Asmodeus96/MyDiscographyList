using System.ComponentModel;
using System.Windows.Media;

namespace MyDiscographyList.Model
{
    public class ArtistStatusModel : INotifyPropertyChanged
    {
        private int _statusId;
        private string _statusLabel;
        private Brush _statusColor;
        private int _nbOfStatus;

        public int StatusId { get { return _statusId; } set { _statusId = value; OnPropertyChanged("StatusId"); } }

        public string StatusLabel { get { return _statusLabel; } set { _statusLabel = value; OnPropertyChanged("StatusLabel"); } }

        public Brush StatusColor { get { return _statusColor; } set { _statusColor = value; OnPropertyChanged("StatusColor"); } }

        public int NbOfStatus { get { return _nbOfStatus; } set { _nbOfStatus = value; OnPropertyChanged("NbOfStatus"); } }


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
