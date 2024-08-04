using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.Services;

public class RestaurantService(RestaurantReservationDBContext restaurantReservationDBContext) : IRestaurantService
{
    private readonly RestaurantReservationDBContext _restaurantReservationDBContext = 
            restaurantReservationDBContext ?? throw new ArgumentNullException(nameof(restaurantReservationDBContext));
    public async Task AddRestaurant(Restaurant newRestaurant)
    {
        await _restaurantReservationDBContext.Restaurants.AddAsync(newRestaurant);
        _restaurantReservationDBContext.ChangeTracker.DetectChanges();
        Console.WriteLine(_restaurantReservationDBContext.ChangeTracker.DebugView.LongView);

        await _restaurantReservationDBContext.SaveChangesAsync();
    }

    public async Task DeleteRestaurant(Restaurant restaurantToDelete)
    {
        var restaurant = await _restaurantReservationDBContext.Restaurants.FindAsync(restaurantToDelete.Id);

        if (restaurant is not null)
        {
            _restaurantReservationDBContext.Restaurants.Remove(restaurant);
            _restaurantReservationDBContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_restaurantReservationDBContext.ChangeTracker.DebugView.LongView);
            await _restaurantReservationDBContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Restaurant not found");
        }
    }

    public async Task EditRestaurant(Restaurant updateRestaurant)
    {
        var restaurant = await _restaurantReservationDBContext.Restaurants.FindAsync(updateRestaurant.Id);

        if(restaurant is not null)
        {
            restaurant.name = updateRestaurant.name;
            restaurant.cuisine = updateRestaurant.cuisine;
            restaurant.borough = updateRestaurant.borough;

            _restaurantReservationDBContext.Restaurants.Update(restaurant);
            _restaurantReservationDBContext.ChangeTracker.DetectChanges();
            Console.WriteLine(_restaurantReservationDBContext.ChangeTracker.DebugView.LongView);
            await _restaurantReservationDBContext.SaveChangesAsync();
        }
        else
        {

           throw new InvalidOperationException("Restaurant not found");
        }
    }

    public async Task<Restaurant?> GetRestaurant(ObjectId id)
    {
        return await _restaurantReservationDBContext.Restaurants.FindAsync(id);
    }

    public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
    {   
        return await _restaurantReservationDBContext.Restaurants.OrderByDescending( c => c.Id).Take(20).AsNoTracking().ToListAsync();
    }
}
