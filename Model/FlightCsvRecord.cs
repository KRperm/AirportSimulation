using CsvHelper.Configuration.Attributes;
using System;

namespace AirportSimulation.Model
{
    public struct FlightCsvRecord
    {
        public const int PLANE_TYPE_INDEX = 0;
        public const int FLIGHT_TYPE_INDEX = 1;
        public const int DATE_INDEX = 2;
        public const int CITY_INDEX = 3;

        [Index(PLANE_TYPE_INDEX)]
        public Plane.Type PlaneType { get; set; }
        [Index(FLIGHT_TYPE_INDEX)]
        public Flight.Type FlightType { get; set; }
        [Index(DATE_INDEX)]
        public DateTime Date { get; set; }
        [Index(CITY_INDEX)]
        public string City { get; set; }
    }
}
