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
        public async Task JoinRoom(string userEmail, string adminEmail)
        {
            try
            {
                var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
                var admin = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == adminEmail);

                if (user == null || admin == null)
                {
                    await Clients.Caller.SendAsync("Not found");
                    return;
                }

                var roomName = GetPrivateRoomName(user.Id, admin.Id);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);


                await Clients.Group(roomName).SendAsync("Userjoin", user.UserName, user.Id);
                var messageHistory = await GetMessageHistory(user.Id, admin.Id);

                await Clients.Caller.SendAsync("ReceviceMessageHistory", messageHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Joinid: {ex.Message}");
                throw;
            }
        }

        public async Task Sendmessage(string senderEmail, string message, string receiverEmail)
        {
            try
            {
                var sender = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == senderEmail);
                var receiver = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == receiverEmail);

                if (sender == null || receiver == null)
                {
                    await Clients.Caller.SendAsync("Not found");
                    return;
                }

                var chatMessage = new Message
                {
                    message = message,
                    UserId = sender.Id,
                    TimeStamp = DateTime.UtcNow,
                    Roomname = GetPrivateRoomName(sender.Id, receiver.Id)
                };

                await _databaseContext.Messages.AddAsync(chatMessage);
                await _databaseContext.SaveChangesAsync();
                var roomname = GetPrivateRoomName(sender.Id, receiver.Id);

                await Clients.Group(roomname).SendAsync("ReceiveMessage", sender.UserName, sender.Id, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Sendmessage: {ex.Message}");
                throw;
            }
        }

        private async Task<List<MessageDTO>> GetMessageHistory(int userId, int adminId)
        {
            var sortedIds = new List<int> { userId, adminId }.OrderBy(id => id).ToArray();
            var roomName = $"PrivateRoom_{sortedIds[0]}_{sortedIds[1]}";

            var messages = await _databaseContext.Messages
                .Where(m => m.Roomname == roomName)
                .OrderBy(m => m.TimeStamp).ToListAsync();

            var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var admin = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == adminId);

            var messageDTOs = messages.Select(m => new MessageDTO
            {
                Id = m.Id,
                message = m.message,
                UserId = m.UserId,
                TimeStamp = m.TimeStamp,
                Roomname = m.Roomname,
                Username = m.UserId == userId ? user.UserName : admin.UserName
            }).ToList();
            return messageDTOs;
        }

        private string GetPrivateRoomName(int userId, int adminId)
        {
            var sortedIds = new List<int> { userId, adminId }.OrderBy(id => id).ToArray();
            return $"PrivateRoom_{sortedIds[0]}_{sortedIds[1]}";
        }
    }
}
