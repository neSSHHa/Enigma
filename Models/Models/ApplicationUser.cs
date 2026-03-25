using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Image { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string Name {  get; set; }

        public int Tokens {  get; set; }

        private readonly HashSet<Lecture> lectures = new HashSet<Lecture>();
        public IReadOnlySet<Lecture> Lectures => lectures;

        public bool assignWorker(Lecture lecture)
        {
            if (lecture == null || lectures.Contains(lecture))
            {
                return false;
            }
            lectures.Add(lecture);
            return true;
        }
        public bool unassignWorker(Lecture lecture)
        {
            if (lecture == null || !lectures.Contains(lecture))
            {
                return false;
            }
            lectures.Remove(lecture);
            return true;
        }
        public void clearWorkers()
        {
            lectures.Clear();

        }

    }
}
