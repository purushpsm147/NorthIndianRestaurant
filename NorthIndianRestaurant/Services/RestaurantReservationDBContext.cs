using Microsoft.EntityFrameworkCore;
using NorthIndianRestaurant.Models;

namespace NorthIndianRestaurant.Services;

public class RestaurantReservationDBContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; init; }


    public DbSet<Reservation> Reservations { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Restaurant>();
        modelBuilder.Entity<Reservation>();
    }
}
