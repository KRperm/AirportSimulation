namespace AirportSimulation.Model
{
    public struct FlightResult
    {
        public Flight Flight { get; set; }
        public int PassangersAmount { get; set; }

        public FlightResult(Flight flight, int passangers)
        {
            Flight = flight;
            PassangersAmount = passangers;
        }

        public override string ToString()
        {
            var flightType = Flight.FlightType == Flight.Type.Arrival ? "Прилёт в" : "Вылет из";
            return $"{flightType} {Flight.City}; Самолет типа {Flight.Plane.PlaneType}; Пассажиры {PassangersAmount} из {Flight.Plane.MaxCapacity}";
        }
    }
}
