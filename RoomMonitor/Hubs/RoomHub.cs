using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace RoomMonitor.Hubs
{
    public class RoomHub : Hub
    {
        public void InitiateTimer(int roomId, int startMinutes)
        {
            Clients.Group(roomId.ToString()).InitiateTimer(startMinutes);
        }

        public void PauseTimer(int roomId)
        {
            Clients.Group(roomId.ToString()).PauseTimer();
        }

        public void StartTimer(int roomId)
        {
            var endTime = DateTime.Now.AddMinutes(20);
            Clients.Group(roomId.ToString()).StartTimer(endTime);
        }

        public void ShowTextClue(int roomId, string clue)
        {
            Clients.Group(roomId.ToString()).showTextClue(clue);
        }

        public void HideTextClue(int roomId)
        {
            Clients.Group(roomId.ToString()).hideTextClue();
        }

        public override Task OnConnected()
        {
            var roomId = Context.QueryString["id"];
            if(roomId != null) Groups.Add(Context.ConnectionId, roomId);
            return base.OnConnected();
        }
    }
}