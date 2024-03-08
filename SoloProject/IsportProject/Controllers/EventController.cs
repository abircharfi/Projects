using System.Xml.Serialization;
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
    if (!IsUserLoggedIn()) return RedirectToIndex(); 
    var CurrentUser = HttpContext.Session.GetInt32("UserId"); 
    User user = _context.Users.FirstOrDefault(u=>u.UserId== CurrentUser);
    var events = _context.Events
                         .Include(e => e.Creator)
                         .Include(e => e.Team)
                         .ToList();

    // Fetch attendances for the current user.
    var attendanceUser = _context.Attendance
                                 .Where(a => a.UserId == user.UserId)
                                 .ToList();

    var userEvents = new List<Event>();

    foreach (var item in attendanceUser)
    {
        var eventToAdd = _context.Events
                                 .Where(e => e.EventDate == DateTime.Today)
                                 .Include(e => e.Creator) 
                                 .Include(e => e.Team)    
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

// Display event 
[HttpGet("event/{EventId}")]
public IActionResult showOne(int EventId, string infoType = null)
{
    if (!IsUserLoggedIn()) return RedirectToIndex();
    Event selectedEvent = _context.Events
                                  .Include(e => e.Creator)
                                  .Include(e => e.Team)
                                  .FirstOrDefault(p => p.EventId == EventId);
              

    // current user
    var User = HttpContext.Session.GetInt32("UserId");
    ViewBag.UserId = User;

    // Team 
    var attendances = _context.Attendance.Where(a => a.EventId == EventId).ToList();
    var userIds = attendances.Select(a => a.UserId).ToList();
    var usersInEvent = _context.Users.Where(u => userIds.Contains(u.UserId)).ToList();
 
    if (infoType != null)
    {
        ViewBag.InfoType = infoType;
    }
    ViewBag.users = usersInEvent;

    return View(selectedEvent);
}

// Event info 
[HttpGet("EventInfo/{EventId}/{infoType}")]
public IActionResult ShowInfo(int EventId, string infoType)
{
    
    Event selectedEvent = _context.Events
                                  .Include(e => e.Creator)
                                  .Include(e => e.Team)
                                  .FirstOrDefault(p => p.EventId == EventId);

    
    if (infoType != null)
    {
        ViewBag.InfoType = infoType;
    }

    return View("showOne", selectedEvent);
}


// Search Event 
[HttpGet("search")]

public IActionResult Search(string searchOption, string searchValue)
{
    if (!IsUserLoggedIn()) return RedirectToIndex();  
    var events = _context.Events
                         .Include(e => e.Creator)
                         .Include(e => e.Team)
                         .ToList();

    if (searchValue != null)
         {
            if(searchOption == "name")
            {
             var SearchByName = _context.Events
                                        .Where(e => e.EventName
                                        .Contains(searchValue))
                                        .Include(e => e.Creator)
                                        .Include(e => e.Team).ToList(); 
             events = SearchByName;
            }
            if(searchOption == "creator")
            {
             var SearchByCreator = _context.Events
                                           .Where(e => e.Creator.FirstName
                                           .Contains(searchValue))
                                           .Include(e => e.Creator)
                                           .Include(e => e.Team)
                                           .ToList(); 

              events = SearchByCreator;
            }
            if(searchOption == "date")
            {
              DateTime searchDate;
            if (DateTime.TryParse(searchValue, out searchDate))
            {
                var SearchByDate = _context.Events
                                           .Where(e => e.EventDate.Date == searchDate.Date)
                                           .Include(e => e.Creator)
                                           .Include(e => e.Team)
                                           .ToList(); 
             events = SearchByDate;
            }
             else
            {
            ModelState.AddModelError("searchValue", "Invalid date format. Please enter a valid date.");
            ViewData["DateError"] = "Invalid date format. Please enter a valid date.";
            }
            }
         }

        return View(events);  
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