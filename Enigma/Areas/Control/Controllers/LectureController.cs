using DataAccess.Repository.IRepository;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Models.ViewModels;
using System.Security.Claims;
using Utility;

namespace Enigma.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize]
    public class LectureController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _email;
        private readonly IHostEnvironment _host;
        private readonly IZoomMeeting _zoomMeetings;
        

        public LectureController(IUnitOfWork unitOfWork, IEmailSender email, IHostEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _email = email;
            _host = host;
            //_zoomMeetings = zoomMeetings;
        }



        public async Task<IActionResult> Lectures(LectureFiltering? oldlf)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            if (oldlf.Start == null && oldlf.End == null)
            {
                oldlf.Start = DateTime.Now;
                var tempDate = DateTime.Now;
                oldlf.End = DateTime.Now.AddMonths(6);
            }
            if (oldlf.Start.Value.Year < 2000 && oldlf.End.Value.Year < 2000)
            {
                oldlf.Start = DateTime.Parse("10.10.2015");
                oldlf.End = DateTime.Parse("10.10.2025");

            }
            // check if user is admin
            if (User.IsInRole(StaticDetails.AdminRole))
            {
                var lectures = _unitOfWork.Lecture.GetAll(included: "Lecturer,Students");
                if (!string.IsNullOrEmpty(oldlf.Search))
                {
                    lectures = lectures.Where(x => x.Name.Contains(oldlf.Search) || x.Description.Contains(oldlf.Search));
                }
                if (!string.IsNullOrEmpty(oldlf.Status))
                {
                    lectures = lectures.Where(x => x.LectureStatus == oldlf.Status);
                }
                if (!string.IsNullOrEmpty(oldlf.Type))
                {
                    lectures = lectures.Where(x => x.Type == oldlf.Type);
                }
                if (oldlf.Start != null)
                {
                    lectures = lectures.Where(x => x.LectureStartTime >= oldlf.Start);
                }
                if (oldlf.End != null)
                {
                    lectures = lectures.Where(x => x.LectureEndTIme <= oldlf.End);
                }

                if(oldlf.MaxLimit <= 0)
                {
                    oldlf.MaxLimit = 30;
                }
                else
                {
                    lectures = lectures.Where(x => x.StudentLimit <= oldlf.MaxLimit);

                }

                oldlf.Lectures = lectures;
                return View(oldlf);
            }
            else
            {
                var lectures = _unitOfWork.Lecture.GetAll(included: "Students,ApplicationUser");
                var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value);
                lectures = lectures.Where(lecture => !lecture.Students.Any(student => student.Id == user.Id));
                if (!string.IsNullOrEmpty(oldlf.Search))
                {
                    lectures = lectures.Where(x => x.Name.Contains(oldlf.Search) || x.Description.Contains(oldlf.Search));
                }
                lectures = lectures.Where(x => x.LectureStatus == StaticDetails.Lecture_Status_Awaits && x.Type == StaticDetails.Lecture_Type_Open);

                if (oldlf.Start != null)
                {
                    lectures = lectures.Where(x => x.LectureStartTime >= oldlf.Start);
                }
                if (oldlf.End != null)
                {
                    lectures = lectures.Where(x => x.LectureEndTIme <= oldlf.End);
                }
                if (oldlf.MaxLimit <= 0)
                {
                    oldlf.MaxLimit = 30;
                }
                else
                {
                    lectures = lectures.Where(x => x.StudentLimit <= oldlf.MaxLimit);

                }
                oldlf.Lectures = lectures;
                return View(oldlf);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LecturesPOST(LectureFiltering? oldlf)
        {
            return RedirectToAction(nameof(Lectures), oldlf);
        }

        [Authorize(Roles = StaticDetails.AdminRole)]
        public async Task<IActionResult> EditLecture(int id)
        {
            var lecture = await _unitOfWork.Lecture.FirstOrDefaultAsync(x => x.Id == id);
            if(lecture != null) 
                return View(lecture);

            return NotFound();
        }
        [Authorize(Roles = StaticDetails.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecturePOST(Lecture lecture)
        {
            lecture.LastModifiedDate = DateTime.Now;
            await _unitOfWork.Lecture.UpdateAsync(lecture);
            await _unitOfWork.SaveAsyncI();
            return RedirectToAction(nameof(Lectures));
        }
        [Authorize(Roles = StaticDetails.AdminRole)]
        public IActionResult NewLecture()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticDetails.AdminRole)]
        public async Task<IActionResult> NewLecturePost(Lecture lecture)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            lecture.LecturerId = claim.Value;
            lecture.Type = StaticDetails.Lecture_Type_Open;
            lecture.DateCreated = DateTime.Now;
            lecture.LastModifiedDate = DateTime.Now;
            lecture.LectureStatus = StaticDetails.Lecture_Status_Awaits;
            await _unitOfWork.Lecture.addAsync(lecture);
            await _unitOfWork.SaveAsyncI();
            return RedirectToAction(nameof(Lectures));

        }

        public async Task<IActionResult> YourLectures()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value,"Lectures");
            var lectures = user.Lectures.Where(x => x.LectureStatus == StaticDetails.Lecture_Status_Awaits);
            return View(lectures);
        }
        // apies

        public async Task<IActionResult> Apply(int Id)
        {
            var lecture = await _unitOfWork.Lecture.FirstOrDefaultAsync(x => x.Id == Id, included: "Students");
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value, "Lectures");
            if (lecture != null && lecture.Type != StaticDetails.Lecture_Type_Occupaed
                && lecture.LectureStatus != StaticDetails.Lecture_Status_Finished
                && lecture.LectureStatus != StaticDetails.Lecture_status_Canceled
                && !lecture.Students.Contains(user))
            {
                if (DateTime.Now > lecture.LectureStartTime.AddHours(StaticDetails.TimeLimit))
                    ; // send email

              
                lecture.assignWorker(user);
                user.assignWorker(lecture);
              
                if(lecture.Students.Count >= lecture.StudentLimit)
                    lecture.Type = StaticDetails.Lecture_Type_Occupaed;
                user.Tokens -= 1;
                await _unitOfWork.Lecture.UpdateAsync(lecture);
                _unitOfWork.ApplicationUser.Update(user);
                await _unitOfWork.SaveAsyncI();
                await _email.SendEmailAsync(user.Email, 
                    "new lectur",
                    "<!DOCTYPE html>\r\n<html>\r\n<body style='margin: 0; padding: 0; font-family: Segoe UI, Arial, sans-serif;'>\r\n    <div style='max-width: 600px; margin: 20px auto; padding: 40px;'>\r\n        <div style='color: #2E4053; text-align: center;'>\r\n            <h1 style='margin: 0 0 20px 0'>🎉 Enrollment Successful!</h1>\r\n            <div style='background-color: #4CAF50; color: white; padding: 8px 20px; \r\n                        border-radius: 20px; display: inline-block; margin: 20px 0;'>\r\n                {lecture.Name}\r\n            </div>\r\n        </div>\r\n        \r\n        <div style='background: #F8F9FA; padding: 30px; border-radius: 15px; margin-top: 20px;'>\r\n            <h2 style='color: #2E4053; margin-top: 0'>Hi {user.FirstName},</h2>\r\n            <p style='color: #6C757D; line-height: 1.6; margin: 0 0 25px 0;'>\r\n                Your enrollment for <strong>{lecture.Name}</strong> has been confirmed.<br>\r\n                <span style='color: #2196F3;'>1 Token</span> has been deducted from your account.\r\n            </p>\r\n            \r\n            <div style='background: white; padding: 20px; border-radius: 10px; margin: 20px 0;'>\r\n                <h3 style='color: #2E4053; margin-top: 0;'>Lecture Details</h3>\r\n                <p style='color: #6C757D; margin: 10px 0;'>\r\n                    🕒 Starts: {lecture.LectureStartTime.ToString(\"MMMM dd, yyyy HH:mm\")}\r\n                </p>\r\n                <p style='color: #6C757D; margin: 10px 0;'>\r\n                    📍 Lecturer: {lecture.Lecturer?.FullName ?? \"TBA\"}\r\n                </p>\r\n                <p style='color: #6C757D; margin: 10px 0;'>\r\n                    🔢 Student Limit: {lecture.StudentLimit} spots\r\n                </p>\r\n            </div>\r\n\r\n            <div style='text-align: center; margin: 25px 0;'>\r\n                <a href='{ _host.ContentRootPath }/lectures/{lecture.Id}' \r\n                   style='background: #2196F3; color: white; padding: 12px 30px; \r\n                          border-radius: 8px; text-decoration: none; display: inline-block;'>\r\n                    View Lecture Details →\r\n                </a>\r\n            </div>\r\n\r\n            <p style='color: #6C757D; margin-bottom: 25px; font-size: 14px;'>\r\n                Need to cancel? Contact our <a href='https://yourdomain.com/support' \r\n                style='color: #2196F3; text-decoration: none;'>support team</a> \r\n                at least {StaticDetails.TimeLimit} hours before the lecture.\r\n            </p>\r\n        </div>\r\n\r\n        <div style='margin-top: 30px; text-align: center; color: #6C757D; font-size: 12px;'>\r\n            <p>© {DateTime.Now.Year} Your Learning Platform. All rights reserved.</p>\r\n            <p>123 Education Street, Knowledge City</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>");
                return RedirectToAction(nameof(Lectures));
                // send email with info.
                

            }

            return RedirectToAction(nameof(Lectures));
        }
        public async Task<IActionResult> Cancel(int Id)
        {
            var lecture = await _unitOfWork.Lecture.FirstOrDefaultAsync(x => x.Id == Id, included: "Students");
            if (lecture != null && lecture.LectureStatus == StaticDetails.Lecture_Status_Awaits)
            {
                var claims = (ClaimsIdentity)User.Identity;
                var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
                var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value);

                // check if user is even participating
                if (lecture.Students.Contains(user))
                {
                    if (lecture.StudentLimit == 1)
                    {
                        if (DateTime.Now < lecture.LectureStartTime.AddHours(StaticDetails.TimeLimit))
                        {
                            user.Tokens += 1;
                            lecture.unassignWorker(user);
                            user.unassignWorker(lecture);
                            // send email 
                        }
                        else
                        {
                            lecture.unassignWorker(user);
                            user.unassignWorker(lecture);
                            lecture.LectureStatus = StaticDetails.Lecture_status_Canceled;
                            // send email
                        }
                    }
                    else
                    {
                        if (DateTime.Now < lecture.LectureStartTime.AddHours(StaticDetails.TimeLimit))
                        {
                            user.Tokens += 1;
                            lecture.unassignWorker(user);
                            user.unassignWorker(lecture);
                            // send email 
                        }
                        else
                        {
                            lecture.unassignWorker(user);
                            user.unassignWorker(lecture);
                            // send email
                        }
                    }
                    await _email.SendEmailAsync(user.Email,
                  "Class clancellation",
                    "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <style>\r\n        body { font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 0; }\r\n        .container { max-width: 600px; margin: 20px auto; padding: 40px; }\r\n        .header { color: #2E4053; text-align: center; }\r\n        .badge { background-color: #4CAF50; color: white; padding: 8px 20px; border-radius: 20px; display: inline-block; margin: 20px 0; }\r\n        .content { background: #F8F9FA; padding: 30px; border-radius: 15px; margin-top: 20px; }\r\n        .cta-button { background: #2196F3; color: white!important; padding: 12px 30px; border-radius: 8px; text-decoration: none; display: inline-block; margin: 25px 0; }\r\n        .footer { margin-top: 30px; text-align: center; color: #6C757D; font-size: 12px; }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class='container'>\r\n        <div class='header'>\r\n            <h1>🎉 You have cancelled your class!</h1>\r\n\r\n        </div>\r\n        \r\n        <div class='content'>\r\n            <h2 style='color: #2E4053; margin-top: 0'>Hi there,</h2>\r\n            <p style='color: #6C757D; line-height: 1.6'>\r\n              Maybe your token has been descreesed be cause of your cancellation. Thats be cause you have cancelled your class after 24 hourse of the class time.\r\n            </p>\r\n            \r\n            <div class=\"\"text-center\"\">\r\n                <a href='{{_webHost.WebRootPath}}/control/lectures' class='cta-button'>\r\n                    Start Learning Now →\r\n                </a>\r\n            </div>\r\n\r\n            <p style='color: #6C757D; margin-bottom: 25px'>\r\n                Need help? Reply to this email or visit our <a href='{{_webHost.WebRootPath}}/contact' style='color: #2196F3; text-decoration: none'>support center</a>.\r\n            </p>\r\n        </div>\r\n\r\n        <div class='footer'>\r\n            <p>© 2023 Your App Name. All rights reserved.</p>\r\n            <p>123 Learning Street, Knowledge City, EdTech 4567</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>");
                    
                    await _unitOfWork.Lecture.UpdateAsync(lecture);
                    _unitOfWork.ApplicationUser.Update(user);
                    await _unitOfWork.SaveAsyncI();
                }


            }

            return RedirectToAction(nameof(Lectures));
        }

        [HttpGet]
        public async Task<IActionResult> GetLecture(int id)
        {
            var lecture = await _unitOfWork.Lecture
                .FirstOrDefaultAsync(l => l.Id == id, included: "Students,Lecturer");
            foreach(var item in lecture.Students) {
                item.clearWorkers();
            }
            if (lecture == null)
            {
                return NotFound();
            }

            var lecturer = await _unitOfWork.ApplicationUser
                .FirstOrDefaultAsync(x => x.Id == lecture.LecturerId);

            lecture.Lecturer = lecturer;

            return Ok(lecture);
        }
        [Authorize(Roles = StaticDetails.AdminRole)]  
        public async Task<IActionResult> RemoveLecture(int id)
        {
            var lecture = await _unitOfWork.Lecture.FirstOrDefaultAsync(x => x.Id == id);
            if (lecture != null)
            {
                var students = lecture.Students;
                foreach (var student in students)
                {
                    student.Tokens += 1;
                    _unitOfWork.ApplicationUser.Update(student);
                    // send email to each students...
                    await _email.SendEmailAsync(student.Email, "Class Cancellation", @"<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 0; }
        .container { max-width: 600px; margin: 20px auto; padding: 40px; }
        .header { color: #2E4053; text-align: center; }
        .badge { background-color: #4CAF50; color: white; padding: 8px 20px; border-radius: 20px; display: inline-block; margin: 20px 0; }
        .content { background: #F8F9FA; padding: 30px; border-radius: 15px; margin-top: 20px; }
        .cta-button { background: #2196F3; color: white!important; padding: 12px 30px; border-radius: 8px; text-decoration: none; display: inline-block; margin: 25px 0; }
        .footer { margin-top: 30px; text-align: center; color: #6C757D; font-size: 12px; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>The class has been cancelled</h1>

        </div>
        
        <div class='content'>
            <h2 style='color: #2E4053; margin-top: 0'>Hi there,</h2>
            <p style='color: #6C757D; line-height: 1.6'>
                The class that you enroled has been cancelled. We are sorry for the inconvenience. Your token has been refunded. Please check your account for the refund.
                <br><br>{{lecture.Name}}<br>
                <span class='badge'>{{lecture.Date}}</span>

            </p>
            
            <div class=""text-center"">
                <a href='{{_webHost.WebRootPath}}/control/lectures' class='cta-button'>
                    Start Learning Now →
                </a>
            </div>

            <p style='color: #6C757D; margin-bottom: 25px'>
                Need help? Reply to this email or visit our <a href='{{_webHost.WebRootPath}}/contact' style='color: #2196F3; text-decoration: none'>support center</a>.
            </p>
        </div>

        <div class='footer'>
            <p>© 2023 Your App Name. All rights reserved.</p>
            <p>123 Learning Street, Knowledge City, EdTech 4567</p>
        </div>
    </div>
</body>
</html>");
                }
                _unitOfWork.Lecture.Remove(lecture);
                await _unitOfWork.SaveAsyncI();
                return RedirectToAction(nameof(Lectures));
            }

            return NotFound();
        }
    }
}
