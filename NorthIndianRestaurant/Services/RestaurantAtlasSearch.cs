using MongoDB.Driver;
using NorthIndianRestaurant.Models;

namespace MongoDBRestaurant.Services;

public class RestaurantAtlasSearch(MongoDBSettings settings)
{
    public MongoDBSettings Settings { get; } = settings;

    public async Task<IEnumerable<Restaurant>> Search(string cuisine)
    {
        var client = new MongoClient(Settings.AtlasURI);
        var database = client.GetDatabase(Settings.DatabaseName);

        var restaurants = database.GetCollection<Restaurant>("Restaurants");
        var filter = Builders<Restaurant>.Filter.Text("Cuisine", cuisine);

        return await restaurants.Aggregate().Match(filter).ToListAsync();
    }
    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
        var client = new MongoClient(Settings.AtlasURI);
        var database = client.GetDatabase(Settings.DatabaseName);

        var restaurants = database.GetCollection<Restaurant>("Restaurants");
        var filter = Builders<Restaurant>.Filter.Empty;

        return await restaurants.Find(filter).ToListAsync();
    }

    public async Task<Restaurant?> GetRestaurantAsync(string id)
    {
        var client = new MongoClient(Settings.AtlasURI);
        var database = client.GetDatabase(Settings.DatabaseName);

        var restaurants = database.GetCollection<Restaurant>("Restaurants");
        var filter = Builders<Restaurant>.Filter.Eq("Id", id);

        return await restaurants.Find(filter).FirstOrDefaultAsync();
    }

    public async Task AddRestaurantAsync(Restaurant restaurant)
    {
        var client = new MongoClient(Settings.AtlasURI);
        var database = client.GetDatabase(Settings.DatabaseName);

        var restaurants = database.GetCollection<Restaurant>("Restaurants");

        await restaurants.InsertOneAsync(restaurant);
    }

    public async Task UpdateRestaurantAsync(string id, Restaurant restaurant)
    {
        var client = new MongoClient(Settings.AtlasURI);
        var database = client.GetDatabase(Settings.DatabaseName);

        var restaurants = database.GetCollection<Restaurant>("Restaurants");

        var filter = Builders<Restaurant>.Filter.Eq("Id", id);

        await restaurants.ReplaceOneAsync(filter, restaurant);
    }

    public async Task DeleteRestaurantAsync(string id)
    {
        var client = new MongoClient(Settings.AtlasURI);
        var database = client.GetDatabase(Settings.DatabaseName);

        var restaurants = database.GetCollection<Restaurant>("Restaurants");

        var filter = Builders<Restaurant>.Filter.Eq("Id", id);

        await restaurants.DeleteOneAsync(filter);
    }
}
