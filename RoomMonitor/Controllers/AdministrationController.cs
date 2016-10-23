using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using RoomMonitor.Models;

namespace RoomMonitor.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Index()
        {
            var model = new List<RoomViewModel>();

            using (var db = new RoomMonitorContext())
            {
                var rooms = db.Rooms.ToList();
                model.AddRange(rooms.Select(room => new RoomViewModel
                {
                    Id = room.Id,
                    Name = room.Name,
                    StartMinutes = room.DefaultStartMinutes,
                    AudioFiles = GetAudioList(),
                    ImageFiles = GetImageList()
                }));
            }
            return View(model);
        }

        private List<SelectListItem> GetAudioList()
        {
            var relativeAudioPath = "~/Content/Audio/";
            var audioPath = Server.MapPath(Url.Content(relativeAudioPath));
            var audiofileTypeExtensions = new List<string>
            {
                ".wav",
                ".mp3"
            };
            return Directory.GetFiles(audioPath, "*.*").Where(x =>
            {
                var extension = Path.GetExtension(x);
                return extension != null && audiofileTypeExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            }).Select(file => new SelectListItem
            {
                Text = Path.GetFileName(file),
                Value = new Uri(Request.Url, Url.Content(relativeAudioPath + Path.GetFileName(file))).AbsoluteUri
            }).ToList();
        }


        private List<SelectListItem> GetImageList()
        {
            var relativeImagePath = "~/Content/Images/";
            var imagePath = Server.MapPath(Url.Content(relativeImagePath));
            var imageFileTypeExtensions = new List<string>
            {
                ".jpeg",
                ".gif",
                ".bmp",
                ".png",
                ".jpg"
            };
            return Directory.GetFiles(imagePath, "*.*").Where(x =>
            {
                var extension = Path.GetExtension(x);
                return extension != null && imageFileTypeExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            }).Select(file => new SelectListItem
            {
                Text = Path.GetFileName(file),
                Value = new Uri(Request.Url, Url.Content(relativeImagePath + Path.GetFileName(file))).AbsoluteUri
            }).ToList();
        }}
}