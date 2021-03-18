using DataAccessLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Core.Extensions;
using System.ComponentModel;
using MyDiscographyList.Model;

namespace MyDiscographyList.ViewModel
{
    class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IPieSegmentViewModel> _donutModels;
        private List<ArtistStatusModel> _artistStatusList;

        public DashboardViewModel()
        {
            _artistStatusList = DataAccess.CountStatusDisco();

            _donutModels = new ObservableCollection<IPieSegmentViewModel>();
            

            foreach(var status in _artistStatusList)
            {
                _donutModels.Add(new DonutSegmentViewModel { Value = status.NbOfStatus, Name = status.StatusLabel, Stroke = ToShade(status.StatusColor.ExtractColor(), 0.8), Fill = ToGradient(status.StatusColor.ExtractColor()), StrokeThickness = 2 });
            }

            SegmentSelectionCommand = new ActionCommand<NotifyCollectionChangedEventArgs>(OnSegmentSelectionExecute);
        }

        private void OnSegmentSelectionExecute(NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.IsNullOrEmptyList() && e.NewItems[0] != null)
            {
                var selectedSegment = e.NewItems[0];
                SelectedSegment = (IPieSegmentViewModel)selectedSegment;
            }
        }

        private IPieSegmentViewModel _selectedSegment;

        public IPieSegmentViewModel SelectedSegment
        {
            get { return _selectedSegment; }
            set
            {
                _selectedSegment = value;
                OnPropertyChanged("SelectedSegment");
            }
        }
        // Binds to ItemsSource of Donut Chart
        public ObservableCollection<IPieSegmentViewModel> DonutModels { get { return _donutModels; } }

        // For managing Addition of new segments
        public ActionCommand AddNewItemCommand { get; set; }

        public ActionCommand<NotifyCollectionChangedEventArgs> SegmentSelectionCommand { get; set; }

        // Populates combo box for choosing color of new item to add
        public List<DonutBrushesModel> AllBrushes
        {
            get { return typeof(Brushes).GetProperties().Select(x => new DonutBrushesModel { BrushName = x.Name, Brush = (Brush)x.GetValue(null, null) }).ToList(); }
        }

        


        // Helper functions to create nice brushes out of colors
        private Brush ToGradient(Color baseColor)
        {
            return new LinearGradientBrush(new GradientStopCollection()
            {
                new GradientStop(baseColor, 0.0),
                new GradientStop(ToShade(baseColor, 0.7).Color, 1.0),
            });
        }

        private SolidColorBrush ToShade(Color baseColor, double shade)
        {
            return new SolidColorBrush(Color.FromArgb(baseColor.A, (byte)(baseColor.R * shade), (byte)(baseColor.G * shade), (byte)(baseColor.B * shade)));
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DonutBrushesModel
    {
        public Brush Brush { get; set; }
        public string BrushName { get; set; }
    }

    public class DonutSegmentViewModel : INotifyPropertyChanged, IPieSegmentViewModel
    {
        public double _Value;
        public double _Percentage;
        public bool _IsSelected;
        public string _Name;
        public Brush _Fill;
        public Brush _Stroke;
        public double _strokeThickness;

        public double StrokeThickness { get { return _strokeThickness; } set { _strokeThickness = value; OnPropertyChanged("StrokeThickness"); }}

        public double Value { get { return _Value; } set { _Value = value; OnPropertyChanged("Value"); }}

        public double Percentage { get { return _Percentage; } set { _Percentage = value; OnPropertyChanged("Percentage"); }}

        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnPropertyChanged("IsSelected"); }}

        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); }}

        public Brush Fill { get { return _Fill; } set { _Fill = value; OnPropertyChanged("Fill"); }}

        public Brush Stroke { get { return _Stroke; } set { _Stroke = value; OnPropertyChanged("Stroke"); }}



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
