using System.Collections.Generic;
using System.Web.Mvc;
using RoomMonitor.Models;

namespace RoomMonitor.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Index()
        {
            //todo: get models from database/ef
            var model = new List<RoomViewModel>
            {
                new RoomViewModel
                {
                    Id = 1,
                    Name = "Big Room",
                    StartMinutes = 120
                },
                new RoomViewModel
                {
                    Id = 2,
                    Name = "Little Room",
                    StartMinutes = 60
                }
            };
            return View(model);
        }
    }
}