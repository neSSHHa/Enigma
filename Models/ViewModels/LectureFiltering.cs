using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class LectureFiltering
    {
        public IEnumerable<Lecture> Lectures { get; set; }
        public string Search { get; set; }
        public string Type {  get; set; }
        public int MaxLimit { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Status { get; set; }
        public string Lecturer { get; set; }

    }
}
