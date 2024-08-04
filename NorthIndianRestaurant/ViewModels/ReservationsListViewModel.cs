using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.ViewModels;

public class ReservationsListViewModel
{
    public IEnumerable<Reservation>? Reservations { get; set; }
}
