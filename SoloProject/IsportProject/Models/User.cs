
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using IsportProject.Attributes;
using System.ComponentModel;


namespace IsportProject.Models;
public class User
{

    [Key]
    public int UserId {get; set;}
    [Required(ErrorMessage="First name is required")]
    [MinLength(2, ErrorMessage ="First name should be at least 2 characters ")]
    [DisplayName("First Name" )]
    public string FirstName {get; set;}
    [Required(ErrorMessage="Last name is required")]
    [MinLength(2, ErrorMessage ="Last name should be at least 2 characters ")]
    [DisplayName("Last Name" )]
    public string LastName {get; set;}

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email {get; set;}

    [Required(ErrorMessage="Password is required")]
    [MinLength(8, ErrorMessage ="Password should be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password {get; set;}
    
    [ValidationDateAttribute]
    public string Birthdate {get; set;}
   
    public string? ProfilePicture { get; set; }

    public int Passwordlength { get; set; }

    // Navigation property for Events created by the user
    public List<Event> EventCreated { get; set; } = new List<Event>();

    //The list of Events that a User have Joined

    public List<Attendance> Mygames  {get;set;}  = new List<Attendance>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
}