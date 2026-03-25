using DataAccess.Migrations;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IPopRepository : IRepository<proofOfPayment>
    {
        Task Add(proofOfPayment pop);
    }
}
