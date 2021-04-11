using System;
using System.Diagnostics;

namespace AirportSimulation.Model
{
    [DebuggerDisplay("{Plane.PlaneType}; {FlightType}; {Date}; {City};")]
    public struct Flight
    {
        public enum Type { Arrival, Departure }
        public static readonly int TypeAmount = Enum.GetNames(typeof(Type)).Length;

        public Plane Plane { get; set; }
        public Type FlightType { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
    }
}
