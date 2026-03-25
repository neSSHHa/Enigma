using DataAccess.DataManipulation;
using DataAccess.Migrations;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Repository
{
    public class PopRepository : Repository<proofOfPayment>, IPopRepository
    {
        private readonly ApplicationDbContext _db;
        public PopRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task Add(proofOfPayment pop)
        {
            // Ensure image data exists
            if (pop.Bytes == null || pop.Bytes.Length == 0)
            {
                throw new ArgumentException("Invalid image: No data found.");
            }

            // Validate file extension
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png" };
            if (string.IsNullOrEmpty(pop.FileExtension) || !allowedExtensions.Contains(pop.FileExtension.ToLower()))
            {
                throw new ArgumentException("Invalid file type: Only JPG and PNG are allowed.");
            }

            // Validate file size (5MB max)
            const decimal maxFileSizeB = 5242880;
            if (pop.Size > maxFileSizeB)
            {
                throw new ArgumentException($"File size exceeds {maxFileSizeB}B limit.");
            }

           
            await _db.Pop.AddAsync(pop);
        }


    }
}
