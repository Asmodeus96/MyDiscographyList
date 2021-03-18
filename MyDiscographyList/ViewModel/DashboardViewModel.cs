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
    class DashboardViewModel
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
        }

        // Binds to ItemsSource of Donut Chart
        public ObservableCollection<IPieSegmentViewModel> DonutModels { get { return _donutModels; } }


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
