using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UserUI.Controllers
{
    public class CommonController : Controller
    {
        private readonly IUserService _userService;
        private readonly EmailService _emailService;
        private readonly INotificationService _notificationService;

        public CommonController(IUserService userService, EmailService emailService, INotificationService notificationService)
        {
            _userService = userService;
            _emailService = emailService;
            _notificationService = notificationService;
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
                TempData["ErrorMessage"] = "Email and Password are required.";
                return View();
            }

            var token = await _userService.LoginAsync(email, password, "Customer"); // "Customer" is the default role for users
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
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
                TempData["ErrorMessage"] = "Fill all field!";
                return RedirectToAction("Login");
            }

            // check user exit
            bool userExits = await _userService.CheckUserExists(email);
            if (userExits)
            {
                TempData["ErrorMessage"] = "Email has already been registered";
                return RedirectToAction("Login");
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
        public IActionResult Verify()
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
            // get verify code from session
            string storedCode = HttpContext.Session.GetString("VerifyCode"); // correct code

            if (string.IsNullOrEmpty(storedCode) || verifyCode != storedCode)
            {
                ViewBag.ErrorMessage = "Invalid Verify Code!";
                return View();
            }

            // Get data from TempData
            string fullname = TempData["Fullname"] as string;
            string email = TempData["Email"] as string;
            string address = TempData["Address"] as string;
            string phone = TempData["Phone"] as string;
            string password = TempData["Password"] as string;
            TempData.Keep();

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

        // ***** Profile *****
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Get user token from session
            string token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Common");
            }

            // Get user info
            var user = await _userService.GetUserInfoAsync(token);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login", "Common");
            }

            return View(user);
        }

        // ***** Update Profile *****
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(int userId, string email, string fullname, string phone,
            string address)
        {
            // Get user token from session
            string token = HttpContext.Session.GetString("UserToken");

            // create UpdateUserProfileDTO
            var updateUserProfileDto = new DTOs.UserDTO.UpdateUserProfileDTO
            {
                UserId = userId,
                Email = email,
                FullName = fullname,
                PhoneNumber = phone,
                Address = address,
                Role = "Customer" // User role, default is "Customer"
            };

            // call user service to update profile
            var result = await _userService.UpdateUserProfileAsync(token, updateUserProfileDto);
            if(!result)
            {
                TempData["ErrorUpdateMessage"] = "Failed to update profile. Please try again.";
                return RedirectToAction("Profile", "Common");
            }
            TempData["SuccessUpdateMessage"] = "Profile updated successfully.";

            // Notify profile updated via SignalR
            await _notificationService.NotifyProfileUpdatedAsync(userId);

            return RedirectToAction("Profile", "Common");
        }

        // ***** Forgot Password *****
        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetCode()
        {
            // get email from Form send by Ajax
            string email = Request.Form["email"];

            if (string.IsNullOrEmpty(email))
            {
                TempData["ForgotErrorMessage"] = "Email is required.";
                return RedirectToAction("Forgot", "Common");
            }

            // Check if user exists
            var userExists = await _userService.CheckUserExists(email);
            if (!userExists)
            {
                TempData["ForgotErrorMessage"] = "Email not found.";
                return RedirectToAction("Forgot", "Common");
            }

            // Generate reset code
            string resetCode = new Random().Next(100000, 999999).ToString(); // Mã xác nhận 6 số
            string emailBody = $"Your password reset code is: <b>{resetCode}</b>";
            bool emailSent = await _emailService.SendEmailAsync(email, "Reset Your Password", emailBody);
            if (!emailSent)
            {
                TempData["ErrorMessage"] = "Failed to send reset code. Please try again.";
                return RedirectToAction("Forgot", "Common");
            }

            // Save reset code to session
            HttpContext.Session.SetString("ResetCode", resetCode);
            // Save email to session
            HttpContext.Session.SetString("ResetEmail", email);

            return Content("Mã xác nhận đã được gửi!"); ;
        }

        [HttpPost]
        public IActionResult VerifyResetCode()
        {
            string code = Request.Form["code"];

            // Get reset code from session
            string storedCode = HttpContext.Session.GetString("ResetCode");

            if (string.IsNullOrEmpty(storedCode) || code != storedCode)
            {
                return Content("Mã xác nhận không hợp lệ!");
            }

            return Content("Mã xác nhận hợp lệ!"); // Chuyển hướng đến trang đặt lại mật khẩu
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword()
        {
            // Get email and new password from Form send by Ajax
            string email = HttpContext.Session.GetString("ResetEmail");
            string newPassword = Request.Form["newPassword"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword))
            {
                return Content("Email và mật khẩu mới là bắt buộc.");
            }

            // Call user service to reset password
            var result = await _userService.ResetPasswordAsync(email, newPassword);
            if (!result)
            {
                return Content("Đặt lại mật khẩu không thành công. Vui lòng thử lại.");
            }

            // Xóa reset code khỏi session
            HttpContext.Session.Remove("ResetCode");
            HttpContext.Session.Remove("ResetEmail");

            return Content("Mật khẩu đã được đặt lại thành công! Vui lòng đăng nhập.");
        }
    }
}
