// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Models;

namespace Enigma.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHost;
        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHost = webHost;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        public string Username { get; set; }

        public string OldImage { get; set; }

        public string Email { get; set; }

        public int Tokens { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            [ValidateNever]
            public string Name { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            OldImage = user.Image;
            Email = user.Email;
            Tokens = user.Tokens;
            var name = user.Name;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = name

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
          
           
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
                var setName = await _userManager.UpdateAsync(user);
                if (!setName.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set name.";
                    return RedirectToPage();
                }
            }
            if (file != null)
            {
                var rootItem = _webHost.WebRootPath;
                if (user.Image != null)
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

                if (imageUrl != user.Image)
                {

                    user.Image = imageUrl;
                    var setImage = await _userManager.UpdateAsync(user);
                    if (!setImage.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to image.";
                        return RedirectToPage();
                    }
                }
            }



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
