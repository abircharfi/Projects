using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IsportProject.Models;


namespace IsportProject.Controllers;

public class EventController : Controller
{

    private MyContext _context;

    public EventController(MyContext context)
    {
        _context=context;
    }

    [HttpGet("Dashboard")]
public IActionResult Index()
{
    if (!IsUserLoggedIn()) return RedirectToIndex(); // Make sure this is the intended behavior.

    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null) return RedirectToIndex(); // Assuming RedirectToIndex() redirects appropriately.

    var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
    if (user == null) return NotFound("User not found."); // User not found in the database.

    // Fetch all events including related data.
    var events = _context.Events
                         .Include(e => e.Creator)
                         .Include(e => e.Team)
                         .ToList();

    // Fetch attendances for the current user.
    var attendanceUser = _context.Attendance
                                 .Where(a => a.UserId == userId)
                                 .ToList();

    // Initialize UserEvents as a List<Event>.
    var userEvents = new List<Event>();

    // Populate UserEvents with events the user is attending.
    foreach (var item in attendanceUser)
    {
        var eventToAdd = _context.Events
                                 .Where(e => e.EventDate == DateTime.Today)
                                 .Include(e => e.Creator) // Assuming you also want related data here
                                 .Include(e => e.Team)    // Same as above
                                 .FirstOrDefault(e => e.EventId == item.EventId);

        if (eventToAdd != null)
        {
            userEvents.Add(eventToAdd);
        }
    }

    ViewBag.User = user;
    ViewBag.UserEvents = userEvents;
    return View(events);     
}


// add event 

    [HttpGet("new/event")]
    public IActionResult AddEvent()
    {
        if (!IsUserLoggedIn()) return RedirectToIndex();
        return View();
    }

   [HttpPost]
    public IActionResult CreateEvent(Event NewEvent)
    {
     
    if (ModelState.IsValid)
    {
        _context.Add(NewEvent);
        _context.SaveChanges();
        var attendance = new Attendance 
        {
            UserId = NewEvent.UserId, 
            EventId = NewEvent.EventId
        };
        _context.Attendance.Add(attendance);
        _context.SaveChanges();
       return RedirectToAction("ShowOne", new { EventId = NewEvent.EventId });

    }    
    return View("AddEvent");

    }


// display event 
    [HttpGet("event/{EventId}")]
    public IActionResult showOne(int EventId)
    {
         if (!IsUserLoggedIn()) return RedirectToIndex();
        Event selectedEvent  = _context.Events
                             .Include(e => e.Creator)
                             .Include(e=>e.Team)
                             .FirstOrDefault(p=>p.EventId == EventId);

       // bool like =_context.Likes.Any(l=>l.PostId ==PostId && l.UserId == HttpContext.Session.GetInt32("UserId"));
        var User = HttpContext.Session.GetInt32("UserId");
        ViewBag.UserId = User;
       // ViewBag.Like=like;
        return View(selectedEvent );
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

}