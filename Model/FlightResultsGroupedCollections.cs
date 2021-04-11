using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportSimulation.Model
{
    public class FlightResultsGroupedCollections
    {
        private readonly Dictionary<Flight.Type, SortedDictionary<DateTime, FlightResult>> _FlightResults;

        public bool HasFlightResults => _FlightResults.Any(d => d.Value.Count > 0);

        public FlightResultsGroupedCollections()
        {
            _FlightResults = new Dictionary<Flight.Type, SortedDictionary<DateTime, FlightResult>>
            {
                [Flight.Type.Arrival] = new SortedDictionary<DateTime, FlightResult>(),
                [Flight.Type.Departure] = new SortedDictionary<DateTime, FlightResult>()
            };
        }

        public void AddRange(IEnumerable<FlightResult> flightResults)
        {
            foreach(var item in flightResults)
                _FlightResults[item.Flight.FlightType][item.Flight.Date] = item;
        }

        public int GetLastPassangers(Flight.Type flightType)
        {
            if (_FlightResults[flightType].Count == 0) return 0;
            var last = _FlightResults[flightType].Last();
            return last.Value.PassangersAmount;
        }

        public int[] GetDayPassangersByHour(Flight.Type flightType, DateTime dayDate)
        {
            var result = new int[24];
            for (int i = 0; result.Length > i; i++)
                result[i] = GetHourPassangers(flightType, dayDate, i);
            return result;
        }

        public int GetHourPassangers(Flight.Type flightType, DateTime dayDate, int hour)
        {
            var fromDate = dayDate.Date.AddHours(hour);
            var toDate = fromDate.AddHours(1);
            var hourPassangersAmount = GetDatePassangers(flightType, fromDate, toDate);
            return hourPassangersAmount;
        }

        public int GetDayPassangers(Flight.Type flightType, DateTime dayDate)
        {
            var fromDate = dayDate.Date;
            var toDate = fromDate.AddDays(1);
            var dayPassangersAmount = GetDatePassangers(flightType, fromDate, toDate);
            return dayPassangersAmount;
        }

        public int GetDatePassangers(Flight.Type flightType, DateTime fromDate, DateTime toDate)
        {
            if (_FlightResults[flightType].Count == 0) return 0;
            var passangersAmount = _FlightResults[flightType]
                  .Where(f => f.Value.Flight.Date >= fromDate && f.Value.Flight.Date < toDate)
                  .Sum(f => f.Value.PassangersAmount);
            return passangersAmount;
        }

        public int GetAllTimePassangers(Flight.Type flightType)
        {
            if (_FlightResults[flightType].Count == 0) return 0;
            var allTimePassangersAmount = _FlightResults[flightType].Sum(f => f.Value.PassangersAmount);
            return allTimePassangersAmount;
        }

        public FlightResult GetLastFlightResult()
        {
            if (!HasFlightResults)
                throw new NoFlightResultsException();

            var lastFlights = _FlightResults
                .Where(d => d.Value.Count > 0)
                .Select(d => d.Value.Last().Value);
            
            var maxDate = lastFlights.Max(f => f.Flight.Date);
            var result = lastFlights.First(f => f.Flight.Date == maxDate);
            return result;
        }

        public class NoFlightResultsException : Exception { }
    }
}
