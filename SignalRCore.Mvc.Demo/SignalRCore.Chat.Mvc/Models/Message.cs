using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCore.Chat.Mvc.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        
        public string Body { get; set; }

        public DateTime Time { get; set; }

        public ChatUser FromUser { get; set; }
    }
}
