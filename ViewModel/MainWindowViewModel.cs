using AirportSimulation.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Timers;

namespace AirportSimulation.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        public string _InfoString;
        public string InfoString
        {
            get => _InfoString;
            set => SetProperty(ref _InfoString, value);
        }
        public int TimeMultiplier
        {
            get => _Model.TimeMultiplier;
            set
            {
                if (_Model.TimeMultiplier == value) return;
                _Model.TimeMultiplier = value;
                OnPropertyChanged(); 
            }
        }
        private string _SimulatedDateTime;
        public string SimulatedDateTime
        {
            get => _SimulatedDateTime;
            set => SetProperty(ref _SimulatedDateTime, value);
        }
        public PassengerTableViewModel Arrival { get; set; }
        public PassengerTableViewModel Departure { get; set; }

        public SeriesCollection GraphSeriesCollection { get; private set; }
        public string[] GraphLabels { get; private set; }

        private readonly MainWindowModel _Model;
        private readonly ColumnSeries _ArrivalSeries;
        private readonly ColumnSeries _DepartureSeries;
        public MainWindowViewModel(MainWindowModel model)
        {
            _ArrivalSeries = new ColumnSeries { Title = "Прилет", Values = new ChartValues<int>(Enumerable.Repeat(0, 24)) };
            _DepartureSeries = new ColumnSeries { Title = "Вылет", Values = new ChartValues<int>(Enumerable.Repeat(0, 24)) };
            GraphSeriesCollection = new SeriesCollection { _ArrivalSeries, _DepartureSeries };
            GraphLabels = CreateGraphLabels();
            _Model = model;
            _Model.Timer.Elapsed += TimeCheckPassedEventHandler;
            Arrival = new PassengerTableViewModel(_Model, Flight.Type.Arrival);
            Departure = new PassengerTableViewModel(_Model, Flight.Type.Departure);
        }

        private void TimeCheckPassedEventHandler(object sender, ElapsedEventArgs e)
        {
            InfoString = _Model.FlightResults.HasFlightResults
                    ? $"Последний рейс: {_Model.FlightResults.GetLastFlightResult()}"
                    : "Ни один из рейсов не выполнен";
            SimulatedDateTime = _Model.SimulatedDateTime.ToString();
            Arrival.UpdateValues();
            Departure.UpdateValues();
            UpdateColumnSeries(Flight.Type.Arrival, _ArrivalSeries);
            UpdateColumnSeries(Flight.Type.Departure, _DepartureSeries);
        }

        private void UpdateColumnSeries(Flight.Type flightType, ColumnSeries series)
        {
            var hoursValues = _Model.FlightResults.GetDayPassangersByHour(flightType, _Model.SimulatedDateTime);
            for (int i = 0; series.Values.Count > i; i++)
                series.Values[i] = hoursValues[i];
        }

        public void Dispose()
        {
            _Model.Timer.Elapsed -= TimeCheckPassedEventHandler;
            _Model.Dispose();
        }

        private static string[] CreateGraphLabels()
        {
            var result = new string[24];
            for (int i = 0; result.Length > i; i++)
                result[i] = string.Format("{0:00}", i);
            return result;
        }
    }
}
