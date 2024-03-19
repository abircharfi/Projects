#pragma warning disable CS8600
#pragma warning disable CS8625
#pragma warning disable CS8602
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
  
    var userEvents = _context.Events
                             .Where(e => e.Team.Any(t => t.UserId == CurrentUser))
                             .Include(e => e.Creator)
                             .Include(e => e.Team)
                             .ToList();


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
    if (TempData != null)
    {
    ViewBag.NotificationMessage = TempData["NotificationMessage"] as string;
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

        var notificationMessage = $"New event added: {NewEvent.EventName} on {NewEvent.EventDate}";
       
        if (TempData != null)
        {
          TempData["NotificationMessage"] = notificationMessage;
        }

        return RedirectToAction("Index");
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
    var CurrentUser = HttpContext.Session.GetInt32("UserId");
    ViewBag.CurrentUser = CurrentUser;  
 
    // display chat messages  
    var chatMessages = _context.ChatMessages
        .Where(c=>c.EventId == selectedEvent.EventId)
        .Include(c => c.User)
        .OrderBy(c => c.CreatedAt)
        .ToList();

    ViewBag.ChatMessages = chatMessages;

    var NewMessage = new ChatMessage();

    ViewBag.NewMessage = NewMessage;

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
    // Team info
    var attendances = _context.Attendance.Where(a => a.EventId == EventId).ToList();
    var userIds = attendances.Select(a => a.UserId).ToList();
    var usersInEvent = _context.Users.Where(u => userIds.Contains(u.UserId)).ToList();
    var teamMembers = usersInEvent.Any() ? usersInEvent : null;
    ViewBag.Team = teamMembers ;
   
   // chat part
    var CurrentUser = HttpContext.Session.GetInt32("UserId");
    var chatMessages = _context.ChatMessages
        .Where(c=>c.EventId == selectedEvent.EventId)
        .Include(c => c.User)
        .OrderBy(c => c.CreatedAt)
        .ToList();

    ViewBag.ChatMessages = chatMessages;
    ViewBag.CurrentUser = CurrentUser;

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
// update event 

[HttpPost]
    public IActionResult UpdateEvent(Event EditedEvent)
    {
    
        if (ModelState.IsValid)
        {
            Event oldEvent = _context.Events.FirstOrDefault(p => p.EventId == EditedEvent.EventId);
            oldEvent.EventName = EditedEvent.EventName;
            oldEvent.Location = EditedEvent.Location;
            oldEvent.EventDate = EditedEvent.EventDate;
            oldEvent.AttendeesNumber = EditedEvent.AttendeesNumber;
            oldEvent.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("showOne", new {EventId= EditedEvent.EventId});
        }
        return RedirectToAction("EditEvent");
    }

// Edit event

 [HttpGet("events/edit/{EventId}")]
    public IActionResult EditEvent(int EventId)
    {
      if (!IsUserLoggedIn()) return RedirectToIndex();
      var EventToUpdate = _context.Events.FirstOrDefault(e => e.EventId == EventId);
      var User = HttpContext.Session.GetInt32("UserId");
      ViewBag.UserId = User;
      return View(EventToUpdate);
    }

//delete Event
    
    [HttpGet("Event/delete/{EventId}")]
    public IActionResult DeleteEvent(int EventId)
    {
      if (!IsUserLoggedIn()) return RedirectToIndex();
       var EventToDelete = _context.Events.FirstOrDefault(e => e.EventId == EventId);
        if (EventToDelete != null)
        {
            List<Attendance> relatedAttendance = _context.Attendance.Where(e => e.EventId == EventId).ToList();
            foreach (Attendance a in relatedAttendance.ToList())
            {

                    _context.Attendance.Remove(a);
                    _context.SaveChanges();

            }
            _context.Events.Remove(EventToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    return RedirectToAction("Index");
    }

//------------------------------------------- Join Event 
  [HttpPost]
  public IActionResult JoinEvent(Attendance newJoin)
  {
    _context.Attendance.Add(newJoin);
    _context.SaveChanges();
    return RedirectToAction("showOne", new {EventId = newJoin.EventId});
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