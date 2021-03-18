using MyDiscographyList.ViewModel;
using System;
using System.Windows;

namespace MyDiscographyList.View
{
    public partial class AddArtistModalWindowView : Window
    {
        public AddArtistModalWindowView()
        {
            InitializeComponent();
            AddArtistViewModel vm = new AddArtistViewModel();
            this.DataContext = vm;
            vm.CloseAction = new Action(this.Close);
        }
    }
}
