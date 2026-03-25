using DataAccess.DataManipulation;
using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DataAccess.BackgroundProcess
{
    public class StatusUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public StatusUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var lectureToUpdate = context.Lecture
                        .GetAll(x => x.LectureStatus == StaticDetails.Lecture_Status_Awaits && x.LectureEndTIme <= DateTime.Now).ToList();
                    foreach(var lecture in lectureToUpdate)
                    {
                        lecture.LectureStatus = StaticDetails.Lecture_Status_Finished;
                        await context.Lecture.UpdateAsync(lecture);
                    }
                    if (lectureToUpdate.Any())
                    {
                        await context.SaveAsyncI();
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
