using MongoDB.Bson;
using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.Services;

public interface IRestaurantService
{
    Task<IEnumerable<Restaurant>> GetAllRestaurants();
    Task<Restaurant?> GetRestaurantById(ObjectId id);
    Task AddRestaurant(Restaurant newRestaurant);
    Task EditRestaurant(Restaurant updateRestaurant);
    Task DeleteRestaurant(Restaurant restaurantToDelete);
}
