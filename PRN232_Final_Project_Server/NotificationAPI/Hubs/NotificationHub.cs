using Microsoft.AspNetCore.SignalR;

namespace NotificationAPI.Hubs
{
    public class NotificationHub : Hub
    {
        /**
         * Sends a notification to all connected clients.
         *         * @param message The message to send.
         *                 
         */
        public async Task NotifyProfileUpdated(int userId)
        {
            await Clients.All.SendAsync("ProfileUpdated", userId);
        }
    }
}
