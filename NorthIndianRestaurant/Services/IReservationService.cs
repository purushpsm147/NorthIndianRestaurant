using MongoDB.Bson;
using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.Services;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetAllReservations();
    Task<Reservation?> GetReservationById(ObjectId id);
    Task AddReservation(Reservation newReservation);
    Task EditReservation(Reservation updateReservation);
    Task DeleteReservation(Reservation reservationToDelete);
}
