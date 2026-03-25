using DataAccess.Repository.IRepository;
using Enigma.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Diagnostics;
using System.Security.Claims;
using Utility;

namespace Enigma.Areas.Control.Controllers
{
    [Area("Control")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;   
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Token()
        {
            if (User.IsInRole(StaticDetails.AdminRole))
            {
                return View(_unitOfWork.Pop.GetAll(included: "ApplicationUser"));
            }

            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // API
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFileCollection files)
        {
            if (files.Count > 0)
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            if (memoryStream.Length < 2097152)
                            {
                                var claims = (ClaimsIdentity)User.Identity;
                                var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
                                var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value);

                                if (user != null)
                                {
                                    var newphoto = new proofOfPayment()
                                    {
                                        Bytes = memoryStream.ToArray(),
                                        Description = formFile.FileName,
                                        FileExtension = Path.GetExtension(formFile.FileName),
                                        Size = formFile.Length,
                                        ApplicationUserId = claim.Value,
                                        dateCreated = DateTime.Now
                                    };
                                    await _unitOfWork.Pop.Add(newphoto);
                                    await _unitOfWork.SaveAsyncI();

                                    return Ok(new { message = "Upload successful" }); // ✅ Return JSON response
                                }
                                else
                                {
                                    return BadRequest(new { message = "User is not logged in" });
                                }
                            }
                            else
                            {
                                return BadRequest(new { message = "File is too large" });
                            }
                        }
                    }
                }
            }
            return BadRequest(new { message = "Image not found. Please try again" });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProof(int id)
        {
            if (id != null)
            {
                var proof = await _unitOfWork.Pop.FirstOrDefaultAsync(x => x.Id == id);
                if (proof != null)
                {
                    _unitOfWork.Pop.Remove(proof);
                    await _unitOfWork.SaveAsyncI();
                    return Ok();
                }
                return NotFound();
            }
            return NotFound();

        }

    }
}
