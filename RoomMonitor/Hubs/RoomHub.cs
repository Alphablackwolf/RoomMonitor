using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using RoomMonitor.Models;

namespace RoomMonitor.Hubs
{
    public class RoomHub : Hub
    {
        private RoomMonitorContext db = new RoomMonitorContext();

        private const string AdminGroup = "Administration";

        public void InitiateTimer(int roomId, int startMinutes)
        {
            var room = db.Rooms.Find(roomId);
            room.CurrentStartMinutes = startMinutes;
            room.CurrentEndDateTime = null;
            room.TimerIsGoing = false;
            db.SaveChanges();
            Clients.Group(roomId.ToString()).InitiateTimer(startMinutes);
            Clients.Group(AdminGroup).InitiateAdminTimer(roomId, startMinutes);
        }

        public void PauseTimer(int roomId)
        {
            var room = db.Rooms.Find(roomId);
            room.CurrentPauseTime = DateTime.Now;
            room.TimerIsGoing = false;
            db.SaveChanges();
            Clients.Group(roomId.ToString()).PauseTimer();
            Clients.Group(AdminGroup).PauseAdminTimer(roomId);
        }

        public void StartTimer(int roomId)
        {
            var room = db.Rooms.Find(roomId);
            var endTime = room.CurrentEndDateTime;
            if (room.CurrentEndDateTime.HasValue)
            {
                if (room.CurrentPauseTime.HasValue)
                {
                    var millisecondsRemaining = (room.CurrentEndDateTime.Value - room.CurrentPauseTime.Value).TotalMilliseconds;
                    endTime = DateTime.Now.AddMilliseconds(millisecondsRemaining);
                }
                else
                {
                    if (room.CurrentEndDateTime <= DateTime.Now)
                    {
                        endTime = DateTime.Now.AddMinutes(room.StartMinutes);
                    }
                }
            }
            else
            {
                endTime = DateTime.Now.AddMinutes(room.StartMinutes);
            }
            room.CurrentEndDateTime = endTime;
            room.CurrentPauseTime = null;
            room.TimerIsGoing = true;
            db.SaveChanges();
            Clients.Group(roomId.ToString()).StartTimer(endTime);
            Clients.Group(AdminGroup).StartAdminTimer(roomId, endTime);
        }

        public void ShowTextClue(int roomId, string clue)
        {
            Clients.Group(roomId.ToString()).showTextClue(clue);
        }

        public void HideTextClue(int roomId)
        {
            Clients.Group(roomId.ToString()).hideTextClue();
        }

        public void TimerFinished(int roomId)
        {
            var room = db.Rooms.Find(roomId);
            room.CurrentEndDateTime = null;
            room.CurrentPauseTime = null;
            room.CurrentStartMinutes = 0;
            room.TimerIsGoing = false;
            db.SaveChanges();
            Clients.Group(AdminGroup).TimerFinished(roomId);
        }

        public override Task OnConnected()
        {
            var roomId = Context.QueryString["id"];
            Groups.Add(Context.ConnectionId, roomId ?? AdminGroup);
            return base.OnConnected();
        }

        public void UpdateRoom(int roomId)
        {
            var room = db.Rooms.Find(Convert.ToInt32(roomId));
            if (room.TimerIsGoing)
            {
                if (room.CurrentEndDateTime != null) Clients.Group(roomId.ToString()).StartTimer(room.CurrentEndDateTime.Value.ToUniversalTime());
            }
            else
            {
                if (room.CurrentPauseTime.HasValue)
                {
                    //timer is paused.
                    if (room.CurrentEndDateTime.HasValue)
                    {
                        var millisecondsRemaining = (room.CurrentEndDateTime.Value - room.CurrentPauseTime.Value).TotalMilliseconds;
                        var endTime = DateTime.Now.AddMilliseconds(millisecondsRemaining);
                        Clients.Group(roomId.ToString()).StartTimer(endTime);
                        Clients.Group(roomId.ToString()).PauseTimer();
                    }
                    else
                    {
                        Clients.Group(roomId.ToString()).InitiateTimer(room.StartMinutes);
                    }
                }
                else
                {
                    if (room.CurrentStartMinutes == 0)
                    {
                        //timer has run out.
                        Clients.Group(roomId.ToString()).StartTimer(DateTime.Now);
                    }
                    else
                    {
                        //timer is initiated, but not running.
                        Clients.Group(roomId.ToString()).InitiateTimer(room.StartMinutes);
                    }
                }
            }
        }

        public void PlayAudio(int roomId, string audiopath)
        {
            Clients.Group(roomId.ToString()).PlayAudio(audiopath);
        }

        public void HideImage(int roomId)
        {
            Clients.Group(roomId.ToString()).HideImage();
        }

        public void ShowImage(int roomId, string imagePath)
        {
            Clients.Group(roomId.ToString()).ShowImage(imagePath);
        }
    }
}