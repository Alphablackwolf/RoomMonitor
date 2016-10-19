using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace RoomMonitor.Hubs
{
    [Authorize]
    public class RoomHub : Hub
    {
        public void InitiateTimer(int roomId, int startMinutes)
        {
            Clients.Group(roomId.ToString()).InitiateTimer(startMinutes);
        }

        public override Task OnConnected()
        {
            var roomId = Context.QueryString["id"];
            if(roomId != null) Groups.Add(Context.ConnectionId, roomId);
            return base.OnConnected();
        }
    }
}