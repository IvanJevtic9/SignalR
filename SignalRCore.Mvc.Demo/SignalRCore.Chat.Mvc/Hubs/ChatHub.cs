using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRCore.Chat.Mvc.Data;
using SignalRCore.Chat.Mvc.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCore.Chat.Mvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ChatUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public ChatHub(ApplicationDbContext db, UserManager<ChatUser> userManager)
        {
            this.userManager = userManager;
            dbContext = db;
        }

        public async Task BroadcastFromClient(string message)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(Context.User);

                Message m = new Message()
                {
                    Body = message,
                    Time = DateTime.Now,
                    FromUser = currentUser
                };

                dbContext.Messages.Add(m);
                await dbContext.SaveChangesAsync();

                await Clients.All.SendAsync(
                        "Broadcast",
                        new
                        {
                            body = m.Body,
                            fromUser = m.FromUser.Email,
                            time = m.Time.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)
                        }
                    );
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("HubError", new { error = ex.Message });
            }
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = await userManager.GetUserAsync(Context.User);

            await dbContext.ConnectedUsers.AddAsync(new ConnectedUser
            {
                ConnectionId = Context.ConnectionId,
                User = currentUser,
                TimeConnected = DateTime.Now,
                DateFormat = DateTime.Now.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)
            });
            await dbContext.SaveChangesAsync();

            var connectedUsers = dbContext.ConnectedUsers.Include(p => p.User).Where(p => p.Id != 0).ToList();

            var users = connectedUsers.Select(c => new
            {
                userName = c.User.Email,
                connectionId = Context.ConnectionId,
                connectionTime = DateTime.Now,
                messageDt = DateTime.Now.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)
            });

            await Clients.All.SendAsync("UserConnected", users);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = dbContext.ConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).FirstOrDefault();

            dbContext.ConnectedUsers.Remove(user);
            await dbContext.SaveChangesAsync();

            var connectedUsers = dbContext.ConnectedUsers.Include(p => p.User).Where(p => p.Id != 0).ToList();

            var users = connectedUsers.Select(c => new
            {
                userName = c.User.Email,
                connectionId = Context.ConnectionId,
                connectionTime = DateTime.Now,
                messageDt = DateTime.Now.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)
            });


            await Clients.All.SendAsync("UserDisconnected", new { 
                message = $"User disconnected, ConnectionId: {Context.ConnectionId}",
                connectedUsers = users
            });
        }
    }
}
