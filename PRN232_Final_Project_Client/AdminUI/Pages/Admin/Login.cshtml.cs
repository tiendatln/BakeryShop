using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AdminUI.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input == null)
            {
                ErrorMessage = "Dữ liệu đăng nhập không hợp lệ.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Vui lòng kiểm tra lại thông tin đăng nhập.";
                return Page();
            }

            // Gọi API để xác thực người dùng
            var token = await _userService.LoginAsync(Input.Email, Input.Password);
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return Page();
            }
            // Lưu token vào session hoặc cookie
            HttpContext.Session.SetString("AdminToken", token);

            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
