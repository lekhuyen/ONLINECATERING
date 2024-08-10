using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using USER.API.DTOs;

namespace USER.API.Models
{
    public class ChatHub : Hub
    {
        private readonly DatabaseContext _databaseContext;
        public ChatHub(DatabaseContext dbcontext)
        {
            _databaseContext = dbcontext;
        }
        public async Task JoinRoom(string userEmail,string roomCode)
        {
            try
            {
                var room = await _databaseContext.Rooms
                .FirstOrDefaultAsync(r => r.RoomCode == roomCode);
                if (room == null)
                {
                    await Clients.Caller.SendAsync("InvalidRoom");
                    return;
                }

                var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);

                if (user == null)
                {
                    await Clients.Caller.SendAsync("Not found");
                    return;
                }

                var userRoom = await _databaseContext.Rooms
                  .FirstOrDefaultAsync(u => u.Users.Any(u => u.Id == user.Id));

                if (userRoom == null)
                {
                    userRoom = new Room
                    {
                        RoomCode = roomCode, 
                        Users = new List<User> { user }
                    };
                    _databaseContext.Rooms.Add(userRoom);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    if (!userRoom.Users.Contains(user))
                    {
                        userRoom.Users.Add(user);
                    }

                    
                }

                await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
                await Clients.Group(roomCode).SendAsync("UserJoined", user.UserName, roomCode);
                var messageHistory = await _databaseContext.Messages.Include(m => m.User)
               .Where(m => m.Room.RoomCode == roomCode)
               .OrderBy(m => m.TimeStamp)
               .Select(m => new
               {
                   UserId = m.User.Id,
                   User = m.User.UserName,
                   Message = m.message,
                   RoomCode = roomCode,
                   Timestamp = m.TimeStamp
               }).ToListAsync();
                await Clients.Caller.SendAsync("ReceiveMessageHistory", messageHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Joinid: {ex.Message}");
                throw;
            }
        }
      

        public async Task SendMessage(string email, string message, string roomCode)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (user == null)
            {
                return;
            }
            var room = await _databaseContext.Rooms
                .FirstOrDefaultAsync(r => r.RoomCode == roomCode);
            if (room == null)
            {
                return;
            }
            
            var chatMessage = new Message
            {
                UserId = user.Id,
                RoomId = room.Id,
                message = message,
                TimeStamp = DateTime.UtcNow,

            };
            await _databaseContext.Messages.AddAsync(chatMessage);
            await _databaseContext.SaveChangesAsync();

            await Clients.Group(roomCode)
                   .SendAsync("ReceiMessage", user.UserName,user.Id, message, chatMessage.TimeStamp, roomCode);
            
        }

    }
}
