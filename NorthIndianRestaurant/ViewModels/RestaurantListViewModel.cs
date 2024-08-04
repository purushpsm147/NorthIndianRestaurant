using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.ViewModels;

public class RestaurantListViewModel
{
    public IEnumerable<Restaurant>? Restaurants { get; set; }
}
