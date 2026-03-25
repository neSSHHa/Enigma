using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ZoomMockUp
    {
        public string topic { get; set; }
        public int duration { get; set; }
        public string hostvideo { get; set; }
        public string parvideo { get; set; }
        public string muteStud { get; set; }
        public string allowstud { get; set; }
        public string start_time { get; set; }
        public string AdminEmail { get; set; }
        public IEnumerable<string> Participants { get; set; }
    }
}
