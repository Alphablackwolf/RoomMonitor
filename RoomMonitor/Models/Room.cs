using System;

namespace RoomMonitor.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DefaultStartMinutes { get; set; }

        public int CurrentStartMinutes { get; set; }

        public int StartMinutes => CurrentStartMinutes == 0 ? DefaultStartMinutes : CurrentStartMinutes;

        public DateTime? CurrentEndDateTime { get; set; }

        public DateTime? CurrentPauseTime { get; set; }

        public bool TimerIsGoing { get; set; }
    }
}