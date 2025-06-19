using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UserUI.Controllers
{
    public class CommonController : Controller
    {
        private readonly IUserService _userService;
        private readonly EmailService _emailService;

        public CommonController(IUserService userService, EmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        // ***** login *****
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

        // ***** logout *****
        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa token khỏi session
            HttpContext.Session.Remove("UserToken");

            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }

        // ***** register *****
        [HttpPost]
        public async Task<IActionResult> Register(string fullname, string email, string address, string phone, string password)
        {
            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address)
            || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please fill all fields";
                return View();
            }

            // check user exit
            bool userExits = await _userService.CheckUserExists(email);
            if (userExits)
            {
                ViewBag.Error = "Email has already been registered";
                return View();
            }

            // Lưu thông tin vào TempData để dùng trong Verify
            TempData["Fullname"] = fullname;
            TempData["Email"] = email;
            TempData["Address"] = address;
            TempData["Phone"] = phone;
            TempData["Password"] = password;

            // create verify code and send email
            string verifyCode = new Random().Next(100000, 999999).ToString(); // Mã xác nhận 6 số
            string emailBody = $"Your verification code is: <b>{verifyCode}</b>";

            bool emailSent = await _emailService.SendEmailAsync(email, "Verify Your Account", emailBody);
            if (!emailSent)
            {
                Console.WriteLine("Bug Verify");
            }

            // Save verify code to session
            HttpContext.Session.SetString("VerifyCode", verifyCode);

            // Chuyển hướng verigy sau khi đăng ký thành công
            return RedirectToAction("Verify", "Common");
        }

        [HttpGet]
        public async Task<IActionResult> Verify()
        {
            // get category in header
            /*var categories = (await _categoryRepo.GetAll()).ToList();
            if (categories != null)
            {
                ViewBag.Categories = categories;
            }*/

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify(string verifyCode)
        {
            // Get data from TempData
            string fullname = TempData["Fullname"] as string;
            string email = TempData["Email"] as string;
            string address = TempData["Address"] as string;
            string phone = TempData["Phone"] as string;
            string password = TempData["Password"] as string;

            // get verify code from session
            string storedCode = HttpContext.Session.GetString("VerifyCode"); // correct code

            if (string.IsNullOrEmpty(storedCode) || verifyCode != storedCode)
            {
                ViewBag.ErrorMessage = "Invalid Verify Code!";
                return View();
            }

            // add new user
            var result = await _userService.RegisterAsync(fullname, email, address, phone, password);
            if (result == false)
            {

                ViewBag.ErrorMessage = "Registration failed. Please try again.";
            }
            // Xóa verify code khỏi session
            HttpContext.Session.Remove("VerifyCode");

            // Chuyển hướng đến trang đăng nhập sau khi đăng ký thành công
            return RedirectToAction("Login", "Common");
        }
    }
}
