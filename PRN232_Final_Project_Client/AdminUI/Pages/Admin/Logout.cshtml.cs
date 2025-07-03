using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminUI.Pages.Admin
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            // Xóa token khỏi session hoặc cookie
            HttpContext.Session.Remove("AdminToken");

            // Chuyển hướng về trang đăng nhập
            RedirectToPage("/Admin/Login");
        }
    }
}
