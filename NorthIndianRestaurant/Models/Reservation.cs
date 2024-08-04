using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NorthIndianRestaurant.Models;

[Collection("reservations")]
public class Reservation
{
    public ObjectId Id { get; set; }
    public ObjectId RestaurantId { get; set; }
    public string? RestaurantName { get; set; }

    [Required(ErrorMessage = "The date and time is required to make this reservation")]
    [Display(Name = "Date")]
    public DateTime date { get; set; }

}
