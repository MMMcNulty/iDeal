using System;
using System.Collections.Generic;
using iDeal.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;


namespace iDeal.Controllers
{
    public class LogRegController : Controller
    {
        private MyContext _context;

        public LogRegController(MyContext context)
        {
            _context = context;
        }

        public void setUserInSession(User sessionUser = null)
        {
            HttpContext.Session.SetInt32("userId", (int)sessionUser.UserId);
            HttpContext.Session.SetString("firstName", sessionUser.FirstName);
            HttpContext.Session.SetString("lastName", sessionUser.LastName);
            HttpContext.Session.SetString("email", sessionUser.Email);
            HttpContext.Session.SetInt32("chipValue", sessionUser.ChipValue);
        }

        public User getUserFromSession()
        {
            var user = new User();
                user.UserId = HttpContext.Session.GetInt32("userId") ?? -1;
                user.FirstName = HttpContext.Session.GetString("firstName");
                user.LastName = HttpContext.Session.GetString("lastName");
                user.Email = HttpContext.Session.GetString("email");
                user.ChipValue= HttpContext.Session.GetInt32("chipValue") ?? 0;
                return user;
        }

        [HttpGet("")]
        public ViewResult Index()
        {
            return View("Index");
        }

        [HttpPost("register")]
        public IActionResult Register(User userFromForm)
        {
            if(ModelState.IsValid)
            {
                if(_context.User.Any(user => user.Email == userFromForm.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return Index();
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    userFromForm.Password = Hasher.HashPassword(userFromForm, userFromForm.Password);
                    userFromForm.ChipValue = 2000;
                    _context.Add(userFromForm);
                    _context.SaveChanges();
                    var userInDb = _context.User.FirstOrDefault(user => user.Email == userFromForm.Email);
                    this.setUserInSession(userInDb);
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            else
            {
                return Index();
            }
        }


        [HttpPost("submitLogin")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _context.User.FirstOrDefault(user => user.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid email, please try again or register for access.");
                    return Index();
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Invalid password, please try again.");
                        return Index();
                    }
                    else
                    {
                        this.setUserInSession(userInDb);
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
            }
            else
            {
                return View("index");
            }
        }

        
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}