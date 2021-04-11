using System;
using System.Linq;
using System.Timers;

namespace AirportSimulation.Model
{
    public class MainWindowModel : IDisposable
    {
        private const string FLIGHTS_FILE_PATH = ".\\schedule.csv";
        private const int REALTIME_INTERVAL = 1000;
        private const int TIME_MULTIPLIER_MAX = 10000;
        private const int TIME_MULTIPLIER_MIN = 1;

        private readonly FlightsCollection _Schedule;
        private readonly PlaneService _PlaneService;
        public FlightResultsGroupedCollections FlightResults { get; private set; }
        private int _TimeMultiplier;
        public int TimeMultiplier
        {
            get => _TimeMultiplier;
            set
            {
                if (value <= TIME_MULTIPLIER_MIN) _TimeMultiplier = TIME_MULTIPLIER_MIN;
                else if (value >= TIME_MULTIPLIER_MAX) _TimeMultiplier = TIME_MULTIPLIER_MAX;
                else _TimeMultiplier = value;
            }
        }
        public DateTime SimulatedDateTime { get; private set; }
        private DateTime _PreviousSimulatedDateTime;

        public Timer Timer { get; private set; }

        public MainWindowModel()
        {
            _PlaneService = new PlaneService();
            SimulatedDateTime = DateTime.MinValue;
            _PreviousSimulatedDateTime = DateTime.MinValue;

            _Schedule = new FlightsCollection(_PlaneService);
            FlightResults = new FlightResultsGroupedCollections();

            Timer = new Timer(REALTIME_INTERVAL);
            Timer.Elapsed += TimerElapsedEventHandler;
            TimeMultiplier = TIME_MULTIPLIER_MIN;
        }

        private void TimerElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            _PreviousSimulatedDateTime = SimulatedDateTime;
            SimulatedDateTime = SimulatedDateTime.AddMilliseconds(REALTIME_INTERVAL * TimeMultiplier);
            
            var flightsToEvaluate = _Schedule.Where(f => f.Date > _PreviousSimulatedDateTime && f.Date <= SimulatedDateTime);
            var random = new Random();
            var completedFlights = flightsToEvaluate.Select(f => new FlightResult(f, random.Next(f.Plane.MaxCapacity)));
            FlightResults.AddRange(completedFlights);
        }

        public bool TryLoadSchedule(out FlightsCollection.LoadResult result) => _Schedule.TryLoad(FLIGHTS_FILE_PATH, out result);

        public void Dispose()
        {
            Timer.Elapsed -= TimerElapsedEventHandler;
            Timer.Dispose();
        }
    }
}
