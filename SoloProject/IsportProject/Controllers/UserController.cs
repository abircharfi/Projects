#pragma warning disable CS8602
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using IsportProject.Models;

namespace IsportProject.Controllers;

public class UserController : Controller
{

    private MyContext _context;

    public UserController(MyContext context)
    {
        _context=context;
    }

    public IActionResult Index()
    {
        ViewData["HideNavbar"] = true;
        return View();
    }

    // Register
    [HttpPost]
    public IActionResult CreateUserForm(User NewUser)
    {
      if(ModelState.IsValid)
      {
        if(_context.Users.Any(u=>u.Email == NewUser.Email))

        {
            ModelState.AddModelError("Email","User already exist !");
            ViewData["HideNavbar"] = true;
            return View("Index");
        }
        NewUser.Passwordlength=NewUser.Password.Length;
        PasswordHasher<User> Hasher = new PasswordHasher<User>();
        NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
        _context.Add(NewUser);
        _context.SaveChanges();
        HttpContext.Session.SetInt32("UserId", NewUser.UserId);
        return RedirectToAction("Index", "Event");
      
        
      }
      ViewData["HideNavbar"] = true;
      return View("Index");
    }

    // Login

    [HttpPost]
    public IActionResult LoginUserForm(UserLogin exUser)
    {
        if(ModelState.IsValid)
        {
            var userInDB = _context.Users.FirstOrDefault(u => u.Email == exUser.LoginEmail);
            if(userInDB == null)
            {
                ModelState.AddModelError("LoginEmail","Invalid Login");
                ViewData["HideNavbar"] = true;
                return View("Index");
            }
            var hasher = new PasswordHasher<UserLogin>();
            var ComparePassword = hasher.VerifyHashedPassword(exUser, userInDB.Password , exUser.LoginPassword);

            if (ComparePassword == 0)
            {
                ModelState.AddModelError("LoginPassword","Invalid Login");
                ViewData["HideNavbar"] = true;
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", userInDB.UserId);            
            return RedirectToAction("Index","Event");
        }
        ViewData["HideNavbar"] = true;
        return View ("Index");
        
    }


    // Logout
    public IActionResult Logout()
        {
            
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index");
    }

    // showUser
   [HttpGet("users/{UserId}")]
    public IActionResult showUser(int UserId)
    {
      var CurrentUser = _context.Users.Include(u=>u.EventCreated).FirstOrDefault(u=>u.UserId == UserId );
      string maskedPassword = new string('*', CurrentUser.Passwordlength);
      CurrentUser.Password = maskedPassword;
      return View(CurrentUser);
    }
    // update user
    [HttpPost]
    public IActionResult UpdateUser(User EditedUser)
    {
    
        if (ModelState.IsValid)
        {
            User oldUser = _context.Users.FirstOrDefault(u => u.UserId == EditedUser.UserId);
            oldUser.FirstName = EditedUser.FirstName;
            oldUser.LastName = EditedUser.LastName;
            oldUser.Email = EditedUser.Email;
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            oldUser.Password = Hasher.HashPassword(oldUser, EditedUser.Password);
            oldUser.Birthdate = EditedUser.Birthdate;
            oldUser.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("showUser", new {UserId= EditedUser.UserId});
        }
        return RedirectToAction("EditUser");
    }

// Edit event

 [HttpGet("users/edit/{UserId}")]
    public IActionResult EditUser(int UserId)
    {
      if (!IsUserLoggedIn()) return RedirectToIndex();
      var UserToUpdate = _context.Users.FirstOrDefault(u => u.UserId == UserId);
      return View(UserToUpdate);
    }

// upload profile pic 
[HttpPost]
public IActionResult UploadProfilePicture(int UserId, IFormFile ProfilePicture)
{
    if (ProfilePicture != null && UserId != null)
    {
        
        var OldUser = _context.Users.Include(u => u.EventCreated).FirstOrDefault(u => u.UserId == UserId);
        if (OldUser != null)
        {
            
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePicture.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                ProfilePicture.CopyTo(stream);
            }

            
            OldUser.ProfilePicture = uniqueFileName; 

            _context.SaveChanges();
        }
        return RedirectToAction("showUser", new { UserId = UserId });
    }
    return RedirectToAction("showUser", new { UserId = UserId });
}


   //---------------------------------IsUserLoggedIn method
    private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

    private IActionResult RedirectToIndex()
        {
            ModelState.AddModelError("LoginPassword", "You should connect!");
            return RedirectToAction("Index", "User");
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
