using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NorthIndianRestaurant.Models;
using NorthIndianRestaurant.Services;
using NorthIndianRestaurant.ViewModels;

namespace NorthIndianRestaurant.Controllers
{
    public class ReservationController(IReservationService reservationService, IRestaurantService restaurantService) : Controller
    {
        private readonly IReservationService _ReservationService = 
            reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        private readonly IRestaurantService _RestaurantService = 
            restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));

        public async Task<IActionResult> Index()
        {
            ReservationsListViewModel viewModel = new()
            {
                Reservations = await _ReservationService.GetAllReservations()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Add(ObjectId restaurantId)
        {
            var selectedRestaurant = await _RestaurantService.GetRestaurantById(restaurantId);

            ReservationAddViewModel reservationAddViewModel = new()
            {
                Reservation = new Reservation()
            };
            reservationAddViewModel.Reservation.RestaurantId = selectedRestaurant.Id;
            reservationAddViewModel.Reservation.RestaurantName = selectedRestaurant.name;
            reservationAddViewModel.Reservation.date = DateTime.UtcNow;

            return View(reservationAddViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReservationAddViewModel reservationAddViewModel)
        {
            Reservation newReservation = new()
            {
                RestaurantId = reservationAddViewModel.Reservation.RestaurantId,
                date = reservationAddViewModel.Reservation.date,
            };

            await _ReservationService.AddReservation(newReservation);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null || string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var selectedReservation = await _ReservationService.GetReservationById(new ObjectId(Id));
            return View(selectedReservation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            try
            {
                var existingReservation = await _ReservationService.GetReservationById(reservation.Id);
                if (existingReservation is not null)
                {
                    await _ReservationService.EditReservation(reservation);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"Reservation with ID {reservation.Id} does not exist!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the reservation failed, please try again! Error: {ex.Message}");
            }

            return View(reservation);
        }

        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null || string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var selectedReservation = await _ReservationService.GetReservationById(new ObjectId(Id));
            return View(selectedReservation);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Reservation reservation)
        {
            if (reservation.Id == null)
            {
                ViewData["ErrorMessage"] = "Deleting the reservation failed, invalid ID!";
                return View();
            }

            try
            {
                await _ReservationService.DeleteReservation(reservation);
                TempData["ReservationDeleted"] = "Reservation deleted successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the reservation failed, please try again! Error: {ex.Message}";
            }

            var selectedRestaurant = await _ReservationService.GetReservationById(reservation.Id);
            return View(selectedRestaurant);
        }
    }
}
