using System.Collections.Generic;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        bool SaveAll();
        void AddTrip(Trip newTrip);
<<<<<<< HEAD
        Trip GetTripByName(string tripName);
        void AddStop(string tripName,Stop newStop);
=======
        Trip GetTripByName(string tripName, string userName);

        void AddStop(string tripName,string userName, Stop newStop);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
>>>>>>> 036c636763d4c51ac646a581213af65261fc34b3
    }
}