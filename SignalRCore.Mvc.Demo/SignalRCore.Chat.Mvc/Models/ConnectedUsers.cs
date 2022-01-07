using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRCore.Chat.Mvc.Models
{
    public class ConnectedUser
    {
        [Key]
        public int Id { get; set; }

        public string ConnectionId { get; set; }

        [ForeignKey("ChatUser")]

        public ChatUser User { get; set; }

        public DateTime TimeConnected { get; set; }

        public string DateFormat { get; set; }
    }
}
