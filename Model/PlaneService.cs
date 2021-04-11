using System.Collections.Generic;

namespace AirportSimulation.Model
{
    public class PlaneService
    {
        private static readonly Dictionary<Plane.Type, Plane> _Planes = new Dictionary<Plane.Type, Plane>
        {
            [Plane.Type.Unknown] = new Plane(Plane.Type.Unknown, 0),
            [Plane.Type.p10] = new Plane(Plane.Type.p10, 10),
            [Plane.Type.p50] = new Plane(Plane.Type.p50, 50),
            [Plane.Type.p100] = new Plane(Plane.Type.p100, 100),
            [Plane.Type.p200] = new Plane(Plane.Type.p200, 200),
            [Plane.Type.p300] = new Plane(Plane.Type.p300, 300),
        };
        public Plane GetPlaneByType(Plane.Type type) => _Planes[type];
    }
}
