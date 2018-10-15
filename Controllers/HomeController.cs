using System;
using System.Collections.Generic;
using System.Linq;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WeddingPlanner.Controllers
{    
    public class HomeController : Controller
    {
        private WeddingPlannerContext _context;
        public HomeController(WeddingPlannerContext wpcontext)
        {
            _context = wpcontext;
        }
        private User ActiveUser 
        {
            get 
            {
                return _context.user.Where(u => u.user_id == HttpContext.Session.GetInt32("user_id")).FirstOrDefault();
            }
        }
        [HttpGet("")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Register(User user)
        {
            // Check ModelState for Model-defined validations
            if(ModelState.IsValid)
            {
                //var result = DbConnector.Query($"SELECT * FROM users WHERE email = '{user.email}'");
                 var result = _context.user.SingleOrDefault(useremail => useremail.email == user.email);
                //var result = _context.Users.ToList();
                //var numUsers = result.Count;
                if(result != null)
                {
                    ModelState.AddModelError("email", "Email already in use.");
                    return View("Registration");
                }

                 // Hash PW if email is unique
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(user, user.password);
                user.password = hashedPw;
                // user.confirm = hashedPw;
                user.created_at = DateTime.Now;
                user.updated_at = DateTime.Now;;
                _context.Add(user);        
                _context.SaveChanges();

                return RedirectToAction("Login");

              
            }

            return View("Registration");
        }

        [HttpPost("login")]
        public IActionResult PerformLogin(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                // Validate existing email
                var result = _context.user.SingleOrDefault(useremail => useremail.email == user.email);
                //var result = DbConnector.Query($"SELECT * FROM users WHERE email = '{user.email}'");
                //var numUsers = result.Count;
                if(result == null)
                {
                    ModelState.AddModelError("email", "Invalid Email");
                    return View("Login");
                }

                // grab first item in list, access data at key of "password", cast to string
                string hashedPWInDB = result.password; // (string)result[0]["password"];

                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                PasswordVerificationResult vResult = hasher.VerifyHashedPassword(user, hashedPWInDB, user.password);

                if(vResult == 0)
                {
                    ModelState.AddModelError("email", "Invalid Password");
                    return View("Login");
                }


                // grab first item in list, access data at key of "user_id", cast to int
                int userIdInDB = result.user_id; //(int)result[0]["user_id"];

                // store user's id in session
                HttpContext.Session.SetInt32("user_id", userIdInDB);               
                HttpContext.Session.SetString("first_name", result.first_name);
                return RedirectToAction("Dashboard");
                //return Redirect($"/BankAccount/{userIdInDB}");
              
            }
            

            return View("Login");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return View("Login");
        }      

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if(ActiveUser == null)
            {
                return RedirectToAction("Login");
            }

            // if(HttpContext.Session.GetInt32("UserId") == null) {
            //     return RedirectToAction("Login", "User");
            // }
            // bool rsvp = false;
            int thisUser = _context.user                
                .Where(u => u.user_id == HttpContext.Session.GetInt32("user_id"))
                .Select(u => u.user_id)
                .FirstOrDefault();
            User thisUserName = _context.user
                .Where(u => u.first_name == HttpContext.Session.GetString("first_name"))
                .FirstOrDefault();
            List<Wedding> weddings = _context.wedding
                .Include(u => u.Host)
                .Include(u => u.WGuest)
                .ToList();
            // foreach (var w in weddings)
            // {
            //     if(w.WGuest.Exists(g => g.user_id == thisUser))
            //     {
            //         rsvp = true;
            //     }
            // }
            ViewBag.Weddings = weddings;
            ViewBag.thisUser = thisUser;
            ViewBag.name = thisUserName;
            //  ViewBag.rsvp = rsvp;
            return View();
        }
        
        [HttpGet("AddWedding")]
        public IActionResult AddWedding()
        {
            if(ActiveUser == null)
            {
                return RedirectToAction("Login");
            }
           
            return View();
        }

        [HttpPost("AddWedding/new")]
        public IActionResult AddWeddingData(AddWeddingData events)
        {
            if(ActiveUser == null)
            {
                return RedirectToAction("Login");
            }
           
            if(ModelState.IsValid)
            {
                Wedding Wedding = events.TheWedding();
                Wedding.Host = ActiveUser;
                
                _context.Add(Wedding);
                _context.SaveChanges();
               
                 Guest Guest = new Guest();
                Guest.Wedding = Wedding;
                Guest.WGuest = ActiveUser;
                _context.Add(Guest);
                _context.SaveChanges();
               
                return RedirectToAction("Dashboard");
            }
            return View("AddWedding");
        }

        [HttpPost("rsvp")]
        public IActionResult rsvp(int id)
        {
            if(ActiveUser == null)
            {
                return RedirectToAction("Login");
            }
           
            Guest guest = new Guest
            {
                user_id = ActiveUser.user_id,
                wedding_id = id
            };
            _context.Add(guest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpPost("unrsvp")]
        public IActionResult unrsvp(int id)
        {
            Guest delete = _context.guest
                .Where(g => g.wedding_id == id)
                .SingleOrDefault(tg => tg.user_id == ActiveUser.user_id);
                _context.guest.Remove(delete);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
        }
        [HttpPost("delete")]
        public IActionResult Delete(int id)
        {
            Wedding wedding = _context.wedding
                .SingleOrDefault(w => w.wedding_id == id);
            _context.wedding.Remove(wedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Wedding/{wedding_id}")]
        public IActionResult ViewWedding(int wedding_id) {
           if(ActiveUser == null)
            {
                return RedirectToAction("Login");
            }
            Wedding wedding = _context.wedding
                .Include(u => u.Host)
                .Include(u => u.WGuest)
                .SingleOrDefault(w => w.wedding_id == wedding_id);
            
            ViewBag.wedding = wedding;
           
            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       

    }
}