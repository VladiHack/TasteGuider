

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using TasteGuider.Models;
using TasteGuider.Validator;

namespace TasteGuider.Controllers
{
    public class UserController:Controller
    {
        private readonly TasteGuiderDBContext _context;

        public UserController(TasteGuiderDBContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {
            ViewBag.userId = UserIdSupplier.id;
            return View(_context.Users.ToList());
        }
        public IActionResult Create()
        {
            ViewBag.userId = UserIdSupplier.id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            string msg = "";
            if (_context.Users.Count() == 0)
            {
                user.IsAdmin = true;
            }
            else
            {
                user.IsAdmin = false;
            }
            List<User> users = _context.Users.ToList();
            user.Id = users[users.Count() - 1].Id + 1;

            msg = UserValidator.ReturnErrorsCreate(users, user);
            ViewBag.Message = msg;

           if (msg=="")
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public IActionResult Login()
        {
            ViewBag.userId = UserIdSupplier.id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginEntity partUser)
        {
            string msg = $"Няма такъв потребител!";
            User user = _context.Users.FirstOrDefault(e => e.Username == partUser.Username && e.Password == partUser.Password);
            if(user!=null)
            {
                UserIdSupplier.id = user.Id;
                if (user.IsAdmin==true)
                {
                    RoleSupplier.role = "Admin";
                }
                else
                {
                    RoleSupplier.role = "User";
                }
                msg = "Успешно влизане! Вашата роля е "+RoleSupplier.role;
            }
            ViewBag.Message = msg;
            return View();
        }
        public IActionResult Logout()
        {
            RoleSupplier.role = "Empty";
            UserIdSupplier.id = 0;
            return View();
        }
       

        public IActionResult Edit(int id) 
        {
            ViewBag.userId = UserIdSupplier.id;
            var users =_context.Users.AsNoTracking().FirstOrDefault(y => y.Id == id);
            return View(users);
        }
        public IActionResult Delete(int id)
        {
            ViewBag.userId = UserIdSupplier.id;
            var users = _context.Users.FirstOrDefault(x => x.Id == id);
            return View(users);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
         //Когато премахваме потребителя, ще махаме и всички негови ревюта
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                foreach(var review in _context.Reviews.ToList())
                {
                    if(review.UserId==user.Id)
                    {
                        _context.Remove(review);
                    }
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            User oldUser = _context.Users.AsNoTracking().FirstOrDefault(p => p.Id == user.Id);
            List<User> users = _context.Users.AsNoTracking().ToList();
            
            string msg = "";
            msg = UserValidator.ReturnErrorsEdit(users, oldUser, oldUser.Id); 
            ViewBag.Message = msg;
            if (msg == "" && user.Password.Length>=6)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));   
        }
        public  IActionResult Details(int id)
        {
            ViewBag.userId = UserIdSupplier.id;
            var users = _context.Users.FirstOrDefault(y => y.Id == id);
            return View(users);
        }
      
    }
}
