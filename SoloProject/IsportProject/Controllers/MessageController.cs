#pragma warning disable CS8600
#pragma warning disable CS8625
#pragma warning disable CS8602
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IsportProject.Models;


namespace IsportProject.Controllers;

public class MessageController : Controller
{

    private MyContext _context;

    public MessageController(MyContext context)
    {
        _context=context;
    }

// add message

 public IActionResult Addmessage(ChatMessage Newmsg)
{
        
    
        _context.Add(Newmsg);
        _context.SaveChanges();

        var UserMessage = _context.Users.FirstOrDefault(u=>u.UserId == Newmsg.UserId );
        var notificationMessage = $"New Message added by {UserMessage.FirstName} {UserMessage.LastName} ";
       
        if (TempData != null)
        {
          TempData["NotificationMessage"] = notificationMessage;
        }
        return RedirectToAction("ShowOne","Event", new{EventId =Newmsg.EventId} );
       


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
