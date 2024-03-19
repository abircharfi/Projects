
using System.ComponentModel.DataAnnotations;

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