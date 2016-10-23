using System.Data.Entity;

namespace RoomMonitor.Models
{
    public class RoomMonitorContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
    }
}