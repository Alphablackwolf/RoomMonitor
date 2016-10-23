using System.Web.Mvc;
using RoomMonitor.Models;

namespace RoomMonitor.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        public ActionResult Index(int id)
        {
            var model = new RoomViewModel();
            using (var db = new RoomMonitorContext())
            {
                var room = db.Rooms.Find(id);
                model.Id = id;
                model.Name = room.Name;
                model.StartMinutes = room.DefaultStartMinutes;
            }
            return View(model);
        }
    }
}