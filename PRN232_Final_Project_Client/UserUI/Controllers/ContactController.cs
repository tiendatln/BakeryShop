using DTOs.FeedbackDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces;

namespace UserUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public ContactController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: /Contact
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Common");
            }
            var feedbacks = await _feedbackService.GetAllAsync(token);
            return View(feedbacks); 
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(CreateFeedbackDTO dto)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();                     
            var success = await _feedbackService.CreateAsync(dto, token);
            if (!success)
            {
                ModelState.AddModelError("", "Gửi phản hồi thất bại!");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _feedbackService.DeleteAsync(id, token);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateFeedbackDTO dto)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _feedbackService.UpdateAsync(id, dto, token);
            return RedirectToAction("Index");
        }

    }
}
