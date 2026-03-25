using DataAccess.DataManipulation;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LectureRepository : Repository<Lecture>, ILectureRepository
    {
        private readonly ApplicationDbContext _db;
        public LectureRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(Lecture lecture)
        {
            var lectureFromDb = await _db.Lectures.FirstOrDefaultAsync(x => x.Id == lecture.Id);

            if (lectureFromDb != null)
            {
                if (lecture.Name != null)
                {
                    lectureFromDb.Name = lecture.Name;
                }
                if (lecture.Description != null)
                { 
                    lectureFromDb.Description = lecture.Description;
                }
                if (lecture.Type != null)
                {
                    lectureFromDb.Type = lecture.Type;
                }
                if (lecture.StudentLimit != null)
                {
                    lectureFromDb.StudentLimit = lecture.StudentLimit;
                }
                if (lecture.LectureStartTime != null)
                {
                    lectureFromDb.LectureStartTime = lecture.LectureStartTime;
                }
                if (lecture.LectureEndTIme != null)
                {
                    lectureFromDb.LectureEndTIme = lecture.LectureEndTIme;
                }
                if (lecture.LastModifiedDate != null)
                {
                    lectureFromDb.LastModifiedDate = lecture.LastModifiedDate;
                }
                if (lecture.LectureStatus != null)
                {
                    lectureFromDb.LectureStatus = lecture.LectureStatus;
                }
                if (lecture.LecturerId != null)
                {
                    lectureFromDb.LecturerId = lecture.LecturerId;
                }
                _db.Lectures.Update(lectureFromDb);

            }
        }
    }
}
