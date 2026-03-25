using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IZoomMeeting
    {
        public bool CreateMeeting(ZoomMockUp zoomModel);
        public bool ViewMeeting(string id);
        public bool CancelMeeting(string id);
    }
}
