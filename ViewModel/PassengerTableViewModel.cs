using AirportSimulation.Model;

namespace AirportSimulation.ViewModel
{
    public class PassengerTableViewModel : ViewModelBase
    {
        private int _LastFlight;
        public int LastFlight
        {
            get => _LastFlight;
            set => SetProperty(ref _LastFlight, value);
        }
        private int _Day;
        public int Day
        {
            get => _Day;
            set => SetProperty(ref _Day, value);
        }
        private int _AllTime;
        public int AllTime
        {
            get => _AllTime;
            set => SetProperty(ref _AllTime, value);
        }

        private readonly MainWindowModel _Model;
        private readonly Flight.Type _FlightType;
        public PassengerTableViewModel(MainWindowModel model, Flight.Type flightType)
        {
            _Model = model;
            _FlightType = flightType;
        }

        public void UpdateValues()
        {
            LastFlight = _Model.FlightResults.GetLastPassangers(_FlightType);
            Day = _Model.FlightResults.GetDayPassangers(_FlightType, _Model.SimulatedDateTime);
            AllTime = _Model.FlightResults.GetAllTimePassangers(_FlightType);
        }
    }
}
