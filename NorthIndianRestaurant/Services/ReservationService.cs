using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.Services;

public class ReservationService(RestaurantReservationDBContext restaurantReservationDBContext) : IReservationService
{
    private readonly RestaurantReservationDBContext _restaurantReservationDBContext = 
            restaurantReservationDBContext ?? throw new ArgumentNullException(nameof(restaurantReservationDBContext));
    public async Task AddReservation(Reservation newReservation)
    {
        var bookedRestaurant = _restaurantReservationDBContext.Restaurants.Find(newReservation.RestaurantId);
        if (bookedRestaurant is not null)
        {
            newReservation.RestaurantName = bookedRestaurant.name;
            await _restaurantReservationDBContext.Reservations.AddAsync(newReservation);
            _restaurantReservationDBContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_restaurantReservationDBContext.ChangeTracker.DebugView.LongView);

            await _restaurantReservationDBContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Restaurant to be reserved cannot not found.");
        }
    }

    public async Task DeleteReservation(Reservation reservationToDelete)
    {
        var reservation = await _restaurantReservationDBContext.Reservations.FindAsync(reservationToDelete.Id);
        if(reservation is not null)
        {

           _restaurantReservationDBContext.Reservations.Remove(reservation);
            _restaurantReservationDBContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_restaurantReservationDBContext.ChangeTracker.DebugView.LongView);
            await _restaurantReservationDBContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Reservation to delete cannot be not found");
        }
    }

    public async Task EditReservation(Reservation updateReservation)
    {
        var reservation = await _restaurantReservationDBContext.Reservations.FindAsync(updateReservation.Id);
        if(reservation is not null)
        {
            reservation.date = updateReservation.date;
            _restaurantReservationDBContext.Reservations.Update(reservation);
            _restaurantReservationDBContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_restaurantReservationDBContext.ChangeTracker.DebugView.LongView);
            await _restaurantReservationDBContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Reservation to edit cannot be not found");
        }
    }

    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        return await _restaurantReservationDBContext.Reservations.OrderBy(r => r.date).Take(20).AsNoTracking().ToListAsync();
    }

    public async Task<Reservation?> GetReservationById(ObjectId id)
    {
        return await _restaurantReservationDBContext.Reservations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
    }
}
