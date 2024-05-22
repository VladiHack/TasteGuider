

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using TasteGuider.Models;
using TasteGuider.Validator;

namespace TasteGuider.Controllers
{
    public class ReviewController:Controller
    {
        private readonly TasteGuiderDBContext _context;

        public ReviewController(TasteGuiderDBContext context)
        {
            _context=context;
        }

        public IActionResult Index(int id)
        {
            ViewBag.role = RoleSupplier.role;
            ViewBag.id = UserIdSupplier.id;
            RestaurantIdSupplier.Id = id;

            ViewBag.userId = UserIdSupplier.id;
            ViewBag.restaurantId = RestaurantIdSupplier.Id;

            return View(_context.Reviews.Where(p=>p.RestaurantId==RestaurantIdSupplier.Id).ToList());
            
        }
        public IActionResult Create()
        {

            ViewBag.userId = UserIdSupplier.id;
            ViewBag.restaurantId = RestaurantIdSupplier.Id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            review.RestaurantId = RestaurantIdSupplier.Id;
            review.UserId= UserIdSupplier.id;
            List<Review> reviews = _context.Reviews.ToList();
            
            if(reviews.Count==0)
            {
                review.Id = 1;
            }
            else
            {
                review.Id = reviews[reviews.Count() - 1].Id + 1;
            }

        
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = review.RestaurantId });
        }


        public IActionResult Edit(int id) 
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.restaurantId = RestaurantIdSupplier.Id;

            var reviews =_context.Reviews.AsNoTracking().FirstOrDefault(y => y.Id == id);
            return View(reviews);
        }
        public IActionResult Delete(int id)
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.restaurantId = RestaurantIdSupplier.Id;

            var reviews = _context.Reviews.FirstOrDefault(x => x.Id == id);
            return View(reviews);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { id = review.RestaurantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Review review)
        {
           
            
               review.UserId=UserIdSupplier.id;
                int id = review.Id;
                _context.Update(review);
                await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { id = review.RestaurantId });

        }

        public  IActionResult Details(int id)
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.restaurantId = RestaurantIdSupplier.Id;


            var review = _context.Reviews.FirstOrDefault(y => y.Id == id);
            return View(review);
        }
      
    }
}
