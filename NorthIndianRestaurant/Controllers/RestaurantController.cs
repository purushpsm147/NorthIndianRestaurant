using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NorthIndianRestaurant.Models;
using NorthIndianRestaurant.Services;
using NorthIndianRestaurant.ViewModels;

namespace NorthIndianRestaurant.Controllers;

public class RestaurantController(IRestaurantService restaurantService) : Controller
{
    private readonly IRestaurantService _restaurantService = 
        restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
    public async Task<IActionResult> Index()
    {
        RestaurantListViewModel restaurantListViewModel = new()
        {
            Restaurants = await _restaurantService.GetAllRestaurants()
        };
        return View(restaurantListViewModel);
    }
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(RestaurantAddViewModel newRestaurant)
    {
        if (ModelState.IsValid)
        {
            Restaurant restaurant = new()
            {
                name = newRestaurant.Restaurant.name,
                borough = newRestaurant.Restaurant.borough,
                cuisine = newRestaurant.Restaurant.cuisine
            };

            await _restaurantService.AddRestaurant(restaurant);
            return RedirectToAction("Index");
        }
        return View(newRestaurant);
    }

    public async Task<IActionResult> Edit(ObjectId id)
    {
        if(id == null || id == ObjectId.Empty)
        {
            return NotFound();
        }
        Restaurant restaurant = await _restaurantService.GetRestaurantById(id);
        if (restaurant is not null)
        {
            return View(restaurant);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Restaurant updateRestaurant)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _restaurantService.EditRestaurant(updateRestaurant);
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {

            ModelState.AddModelError("", $"Updating restaurant failed: {ex.Message}");
        }
        return View(updateRestaurant);
    }

    public async Task<IActionResult> Delete(ObjectId id)
    {
        if (id == null || id == ObjectId.Empty)
        {
            return NotFound();
        }
        var restaurant = await _restaurantService.GetRestaurantById(id);
        if (restaurant is not null)
        {
            return View(restaurant);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Restaurant restaurantToDelete)
    {
        if(restaurantToDelete.Id == ObjectId.Empty)
        {
            ViewData["ErrorMessage"] = "Deleting Restaurant failed, Invalid Id!";
            return View();
        }
        try
        {
            await _restaurantService.DeleteRestaurant(restaurantToDelete);
            TempData["RestaurantDeleted"] = "Restaurant deleted successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = $"Deleting restaurant failed: {ex.Message}";
            ModelState.AddModelError("", $"Deleting restaurant failed: {ex.Message}");
        }
        var selectedRestaurant = await _restaurantService.GetRestaurantById(restaurantToDelete.Id);
        return View(selectedRestaurant);
    }
}
