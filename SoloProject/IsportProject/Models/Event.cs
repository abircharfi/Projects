
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IsportProject.Attributes; 
using System.ComponentModel;

namespace IsportProject.Models;
public class Event
{
    [Key]
    public int EventId {get; set;}

    [Required(ErrorMessage="Event name is required")]
    [DisplayName("Event Name")]
    public string EventName {get; set;}


    [Required(ErrorMessage = "Location is required")]
    [DisplayName("Location Name")]
    public string Location {get; set;}
    
    [Required(ErrorMessage = "Date is required")]
    [EventDateValidation]
    [Display(Name = "Date")]
    [DataType(DataType.DateTime)]
    public DateTime EventDate {get; set;}

    [Required(ErrorMessage = "Attendees Number is required")]
    [DisplayName("Attendees Number")]
    public int AttendeesNumber {get; set;}

    // Navigation property
    public int UserId {get;set;} 
    public User? Creator { get; set; }

    public List<Attendance> Team  {get;set;}  = new List<Attendance>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
}