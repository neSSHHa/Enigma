using DataAccess.Repository.IRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Models.Models;
using Utility;
using Microsoft.AspNetCore.SignalR;
using DataAccess;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Enigma.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize(Roles = StaticDetails.AdminRole)]
    public class MenageUsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;
        private readonly IHubContext<TokenHub> _hubContext;
        private readonly IEmailSender _email;
        
        public MenageUsersController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost, IHubContext<TokenHub> hubContext, IEmailSender email)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
            _hubContext = hubContext;
            _email = email;
        }


        public IActionResult Users(string? email)
        {
            if (!string.IsNullOrEmpty(email))
                return View(_unitOfWork.ApplicationUser.GetAll(x => x.Email == email));
            else
                return View(_unitOfWork.ApplicationUser.GetAll());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UsersPOST(string? email)
        {
            return RedirectToAction(nameof(Users), new { email = email });
        }

        public async Task<IActionResult> User(string id )
        {
            if(!string.IsNullOrEmpty(id))
            {
                var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
                if(user != null)
                {
                    return View(user);
                }
                else
                {
                    return RedirectToAction(nameof(Users));
                }
            }
            return RedirectToAction(nameof(Users));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(ApplicationUser user, IFormFile file)
        {
            if(user.Id != null)
            {
                var dbUser = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == user.Id);


                if (file != null)
                {
                    var rootItem = _webHost.WebRootPath;
                    if (dbUser.Image != null)
                    {
                        var oldImagePath = Path.Combine(rootItem, user.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var newNameItem = Guid.NewGuid().ToString();
                    var extensionItem = Path.GetExtension(file.FileName);
                    var newPathItem = Path.Combine(rootItem, @"images");

                    using (var FileStream = new FileStream(Path.Combine(newPathItem, newNameItem + extensionItem), FileMode.Create))
                    {
                        await file.CopyToAsync(FileStream);
                    }
                    var imageUrl = @"\images\" + newNameItem + extensionItem;
                    dbUser.Image = imageUrl;
                }
                dbUser.Name = user.Name;
                dbUser.Tokens = user.Tokens;
                dbUser.PhoneNumber = user.PhoneNumber;

                _unitOfWork.ApplicationUser.Update(dbUser);
                await _unitOfWork.SaveAsyncI();
                return RedirectToAction(nameof(User), new { id = user.Id});
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> RemoveUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    _unitOfWork.ApplicationUser.Remove(user);
                    await _unitOfWork.SaveAsyncI();
                    return RedirectToAction(nameof(Users));

                }
                return RedirectToAction(nameof(Users));
            }
            return RedirectToAction(nameof(Users));
        }

        // API
        [HttpPost]
        public async Task<IActionResult> IncTokens(string id)
        {
            // check if user is admin 
            if (!string.IsNullOrEmpty(id))
            {
                await _unitOfWork.ApplicationUser.IncreseToken(id);
                await _unitOfWork.SaveAsyncI();
                var tokens = (await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id)).Tokens;
                await _hubContext.Clients.User(id).SendAsync("ReceiveTokenUpdate", tokens);
                var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
                await _email.SendEmailAsync(user.Email,
                    "You've Received New Tokens! 🎉",
                    $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 20px auto; padding: 40px; }}
        .header {{ color: #2E4053; text-align: center; }}
        .badge {{ background-color: #4CAF50; color: white; padding: 8px 20px; border-radius: 20px; display: inline-block; margin: 20px 0; }}
        .content {{ background: #F8F9FA; padding: 30px; border-radius: 15px; margin-top: 20px; }}
        .cta-button {{ background: #2196F3; color: white!important; padding: 12px 30px; border-radius: 8px; text-decoration: none; display: inline-block; margin: 25px 0; }}
        .footer {{ margin-top: 30px; text-align: center; color: #6C757D; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎉 New Tokens Added!</h1>
            <div class='badge'>+1 Token</div>
        </div>
        
        <div class='content'>
            <h2 style='color: #2E4053; margin-top: 0'>Hi there,</h2>
            <p style='color: #6C757D; line-height: 1.6'>
                We're excited to let you know that new tokens have been added to your account! 
                You can now access exclusive lectures and content using your tokens.
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
</html>
");
                return Json(new { tokens });
            }
            return BadRequest();
       
        }
        [HttpPost]
        public async Task<IActionResult> DecTokens(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _unitOfWork.ApplicationUser.DecreseToken(id);
                await _unitOfWork.SaveAsyncI();
                var tokens = (await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id)).Tokens;
                await _hubContext.Clients.User(id).SendAsync("ReceiveTokenUpdate", tokens);
                return Json(new { tokens });
            }
            return BadRequest();
          
        }
        [HttpGet]
        public async Task<IActionResult> getTokens(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
           

            }
            return BadRequest();
        }
    }
}
