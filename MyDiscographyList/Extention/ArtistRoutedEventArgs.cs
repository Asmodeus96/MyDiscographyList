using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyDiscographyList.Extention
{
    public class ArtistRoutedEventArgs : RoutedEventArgs
    {
        private readonly int _artistId;

        public ArtistRoutedEventArgs(int artistId)
        {
            this._artistId = artistId;
        }

        public int ArtistID 
        {
            get { return _artistId;  }
        }
    }
}
