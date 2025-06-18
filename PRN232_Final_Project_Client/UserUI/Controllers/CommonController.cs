using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UserUI.Controllers
{
    public class CommonController : Controller
    {
        private readonly IUserService _userService;

        public CommonController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return View();
            }

            var token = await _userService.LoginAsync(email, password);
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            // Lưu token vào session
            HttpContext.Session.SetString("UserToken", token);

            Console.WriteLine($"Token: {token}");

            // Chuyển hướng đến trang chính sau khi đăng nhập thành công
            return RedirectToAction("Index", "Home");
        }
    }
}
