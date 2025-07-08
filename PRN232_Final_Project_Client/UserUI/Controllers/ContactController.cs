using DTOs.FeedbackDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Service.Interfaces;
using System.Security.Claims;

namespace UserUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public ContactController(IFeedbackService feedbackService) =>
            _feedbackService = feedbackService;

        /* ----------------- Trang Contact ------------------------------- */
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("UserToken"); // ✅ Lấy token từ session

            Console.WriteLine("Token: " + token);
            foreach (var key in HttpContext.Session.Keys)
            {
                Console.WriteLine($"Session[{key}] = {HttpContext.Session.GetString(key)}");
            }
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Common");

            var list = await _feedbackService.GetAllAsync(token);               // ✅ truyền token

            var customerFeedback = await _feedbackService.GetByUserIdAsync(token);
            if (customerFeedback != null)
                ViewBag.CustomerFeedback = customerFeedback;

            Console.WriteLine("Final Token: " + token);


            return View(list);
        }

        /* ----------------- Submit -------------------------------------- */
        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(CreateFeedbackDTO dto)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var ok = await _feedbackService.CreateAsync(dto, token); // ✅ truyền token
            if (!ok) ModelState.AddModelError("", "Gửi phản hồi thất bại!");

            return RedirectToAction("Index");
        }

        /* ----------------- Update -------------------------------------- */
        [HttpPost]
        public async Task<IActionResult> Update(UpdateFeedbackDTO dto)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _feedbackService.UpdateAsync(dto, token);
            return RedirectToAction("Index");
        }


        /* ----------------- Delete -------------------------------------- */
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _feedbackService.DeleteAsync(token); // ✅ truyền token
            return RedirectToAction("Index");
        }
    }
}
