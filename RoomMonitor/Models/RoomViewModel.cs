using System.Collections.Generic;
using System.Web.Mvc;

namespace RoomMonitor.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StartMinutes { get; set; }

        public List<SelectListItem> AudioFiles { get; set; }

        public List<SelectListItem> ImageFiles { get; set; }
    }
}