using System.Web.Mvc;
using RoomMonitor.Models;

namespace RoomMonitor.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        public ActionResult Index(int id)
        {
            var model = new RoomViewModel
            {
                Id = id,
                Name = "Big Room",
                StartMinutes = 60
            };
            return View(model);
        }
    }
}