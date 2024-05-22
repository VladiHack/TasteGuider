

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using TasteGuider.Models;
using TasteGuider.Validator;

namespace TasteGuider.Controllers
{
    public class RestaurantController:Controller
    {
        private readonly TasteGuiderDBContext _context;

        public RestaurantController(TasteGuiderDBContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.role = RoleSupplier.role;
            return View(_context.Restaurants.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            string msg = "";
            List<Restaurant> restaurants = _context.Restaurants.ToList();
            if(restaurants.Count==0)
            {
                restaurant.Id = 1;
            }
            else
            {
                restaurant.Id = restaurants[restaurants.Count() - 1].Id + 1;
            }

            msg = RestaurantValidator.ReturnErrorsCreate(restaurants, restaurant);
            ViewBag.Message = msg;

           if (msg=="")
            {
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }


        public IActionResult Edit(int id) 
        {
            ViewBag.userId = UserIdSupplier.id;
            var users =_context.Restaurants.AsNoTracking().FirstOrDefault(y => y.Id == id);
            return View(users);
        }
        public IActionResult Delete(int id)
        {
            ViewBag.userId = UserIdSupplier.id;
            var users = _context.Restaurants.FirstOrDefault(x => x.Id == id);
            return View(users);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           //Изтриваме всички ревюта за този ресторант
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                foreach(var review in _context.Reviews.ToList())
                {
                    if(restaurant.Id==review.RestaurantId)
                    {
                        _context.Remove(review);
                    }
                }
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Restaurant restaurant)
        {
            Restaurant oldRestaurant = _context.Restaurants.AsNoTracking().FirstOrDefault(p => p.Id == restaurant.Id);
            List<Restaurant> restaurants = _context.Restaurants.AsNoTracking().ToList();
            
            string msg = "";
            msg = RestaurantValidator.ReturnErrorsEdit(restaurants, oldRestaurant, oldRestaurant.Id); 
            ViewBag.Message = msg;
            if (msg == "")
            {
                int id = restaurant.Id;
                _context.Update(restaurant);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
            
        }

        public  IActionResult Details(int id)
        {
            ViewBag.userId = UserIdSupplier.id;
            var restaurant = _context.Restaurants.FirstOrDefault(y => y.Id == id);
            return View(restaurant);
        }
      
    }
}
