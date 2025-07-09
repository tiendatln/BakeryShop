using DTOs.UserDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Service.Interfaces;
using System.Net.Http.Headers;

namespace AdminUI.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly IUserService _userService;

        public UserManagementModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            // Load page view
        }

        public async Task<IActionResult> OnGetUsersAsync(string keyword, int currentPage, int pageSize)
        {
            var token = HttpContext.Session.GetString("AdminToken");

            // Gọi service và nhận về JSON string
            var jsonResponse = await _userService.GetUsersAsync(token, keyword, currentPage, pageSize);

            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return BadRequest(new { error = "Không thể lấy dữ liệu người dùng từ API." });
            }

            // Trả về JSON string trực tiếp với Content-Type là application/json
            return Content(jsonResponse, "application/json");
        }
    }
}
