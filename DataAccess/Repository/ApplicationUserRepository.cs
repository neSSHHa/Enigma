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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task DecreseToken(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.Tokens--;
            Update(user);
        }

        public async Task IncreseToken(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.Tokens++;
            Update(user);
        }

        public void Update(ApplicationUser applicationUser)
        {
            _db.Users.Update(applicationUser);
        }

    }
}
