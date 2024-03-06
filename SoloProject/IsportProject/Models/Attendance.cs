#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel;
namespace IsportProject.Models;
public class Attendance
    {
        [Key]
        public int AttendanceId {get;set;}
        public int UserId {get;set;}
        public int EventId {get;set;}
        public User? User {get;set;} 
        public Event? Event {get;set;} 
        
    
    }