using System;

namespace MessageBoard.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
