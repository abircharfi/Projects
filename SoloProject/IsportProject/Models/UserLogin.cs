#pragma warning disable CS8618
namespace IsportProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel;

public class UserLogin
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [DisplayName("Login" )]
    public string LoginEmail {get; set;}
    
    [Required(ErrorMessage="Password is required")]
    [DataType(DataType.Password)]
    [DisplayName("Password" )]
    public string LoginPassword {get; set;}

}