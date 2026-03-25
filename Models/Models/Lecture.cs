using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Type { get; set; } // occupaed - open

        public int StudentLimit { get; set; }

        public DateTime LectureStartTime { get; set; }
        public DateTime LectureEndTIme { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string LectureStatus {  get; set; } // finished, awaits, in progress, cancelled 

        public string LecturerId { get; set; }
        [ForeignKey("LecturerId")]
        [ValidateNever]
        public ApplicationUser Lecturer { get; set; }

        private readonly HashSet<ApplicationUser> students = new HashSet<ApplicationUser>();
        public IReadOnlySet<ApplicationUser> Students => students;

        public bool assignWorker(ApplicationUser user)
        {
            if (user == null || students.Contains(user))
            {
                return false;
            }
            students.Add(user);
            return true;
        }
        public bool unassignWorker(ApplicationUser user)
        {
            if (user == null || !students.Contains(user))
            {
                return false;
            }
            students.Remove(user);
            return true;
        }
        public void clearWorkers()
        {
            students.Clear();

        }


    }
}
