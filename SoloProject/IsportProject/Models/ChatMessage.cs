
using System.ComponentModel.DataAnnotations;

namespace IsportProject.Models;

    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public int UserId {get;set;}
        public int EventId {get;set;}
        public User? User {get;set;} 
        public Event? Event {get;set;} 
        public string Message { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

