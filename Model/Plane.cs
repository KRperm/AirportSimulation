using System;

namespace AirportSimulation.Model
{
    public struct Plane
    {
        public enum Type { Unknown, p10, p50, p100, p200, p300 }
        public static readonly int TypeAmount = Enum.GetNames(typeof(Type)).Length;

        public Type PlaneType { get; private set; }
        public int MaxCapacity { get; private set; }

        public Plane(Type type, int maxCapacity)
        {
            PlaneType = type;
            MaxCapacity = maxCapacity;
        }
    }
}
