using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApp_UnderTheHood.Authorization;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Verify the credential
            if (Credential.UserName == "admin" && Credential.Password == "admin")
            {
                // Creating the security context
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2023-01-01")
                };

                var identity = new ClaimsIdentity(claims, "TestCookieAuth");

                await HttpContext.SignInAsync("TestCookieAuth", new(identity), new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                });

                return RedirectToPage("/Index");
            }
            else if (Credential.UserName == "root" && Credential.Password == "root")
            {
                // Creating the security context
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "root"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "RD"),
                    new Claim("Admin", "true"),
                    new Claim("EmploymentDate", "2023-01-01")
                };

                var identity = new ClaimsIdentity(claims, "TestCookieAuth");

                await HttpContext.SignInAsync("TestCookieAuth", new(identity), new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                });

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }    
}
