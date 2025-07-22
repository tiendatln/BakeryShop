namespace AdminUI.Pages.Admin
{
    using DTOs.ProductDTO;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Service.Interfaces;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ProductManagementModel" />
    /// </summary>
    public class ProductManagementModel : PageModel
    {
        #region Fields

        /// <summary>
        /// Defines the _productService
        /// </summary>
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;

        private static bool _isSignalRRunnging = false;


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementModel"/> class.
        /// </summary>
        /// <param name="productService">The productService<see cref="IProductService"/></param>
        public ProductManagementModel(IProductService productService, INotificationService notificationService)
        {
            _productService = productService;
            _notificationService = notificationService;
        }

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the CreateProduct
        /// </summary>
        [BindProperty]
        public CreateProductDTO CreateProduct { get; set; } = new CreateProductDTO();

        /// <summary>
        /// Gets or sets the readProducts
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public List<ReadProductDTO> readProducts { get; set; } = new List<ReadProductDTO>();

        //update product

        /// <summary>
        /// Gets or sets the UpdateProduct
        /// </summary>
        [BindProperty]
        public UpdateProductDTO UpdateProduct { get; set; } = new UpdateProductDTO();

        #endregion

        #region Methods

        /// <summary>
        /// The OnGet
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task<IActionResult> OnGet()
        {
            var token = HttpContext.Session.GetString("AdminToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Admin/Login");
            }

            // Nếu hợp lệ, tiếp tục xử lý logic ở đây
            return Page(); // Trả về trang hiện tại
        }




        // Delete product

        /// <summary>
        /// The OnGetDeleteAsync
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        public async Task<IActionResult> OnGetDeleteAsync(int productId)
        {
            var token = HttpContext.Session.GetString("AdminToken");
            Console.WriteLine($"Token: {token}");
            try
            {       
                await _productService.DeleteProductAsync(productId, token);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Failed to delete product: {ex.Message}");
                return Page();
            }
        }

        /// <summary>
        /// The OnGetProductDetailAsync
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        public async Task<IActionResult> OnGetProductDetailAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return new JsonResult(product);
        }

        /// <summary>
        /// The OnGetProductsByCategoryAsyns
        /// </summary>
        /// <param name="categoryId">The categoryId<see cref="int"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        public async Task<ActionResult> OnGetProductsByCategoryAsyns(int categoryId)
        {
            var product = await _productService.GetProductsByCategoryAsync(categoryId);

            return new JsonResult(product); // Redirect to Index view with category products
        }

        //Search

        /// <summary>
        /// The OnGetSearchProductsAsync
        /// </summary>
        /// <param name="searchTerm">The searchTerm<see cref="string"/></param>
        /// <param name="categoryId">The categoryId<see cref="int"/></param>
        /// <param name="status">The status<see cref="bool"/></param>
        /// <param name="pageNumper">The pageNumper<see cref="int"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        public async Task<IActionResult> OnGetSearchProductsAsync(string searchTerm, int categoryId, int status, double minPrice, double maxPrice, int pageNumber)
        {
            var product = await _productService.SearchProductsOdataAsync(searchTerm, categoryId, status, minPrice, maxPrice, 10, (pageNumber - 1) * 10);
            //var totalPages = (int)Math.Ceiling(product. / 10f);
            return Content(product, "application/json");
        }

        // Create product

        /// <summary>
        /// The OnPostCreateAsync
        /// </summary>
        /// <param name="ImageFile">The ImageFile<see cref="IFormFile"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        public async Task<IActionResult> OnPostCreateAsync(IFormFile ImageFile)
        {
            var token = HttpContext.Session.GetString("AdminToken");
            using var stream = ImageFile.OpenReadStream();
            var createdProduct = await _productService.CreateProductAsync(
                CreateProduct,
                stream,
                token
            );

            if (createdProduct == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create product.");
                return Page();
            }

            return RedirectToPage("ProductManagement");
        }

        /// <summary>
        /// The OnPostUpdateAsync
        /// </summary>
        /// <param name="ImageFile">The ImageFile<see cref="IFormFile"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        public async Task<IActionResult> OnPostUpdateAsync(IFormFile ImageFile)
        {
            
            var token = HttpContext.Session.GetString("AdminToken");
            Console.WriteLine($"Token: {token}");

            using var stream = ImageFile.OpenReadStream();
            var updatedProduct = await _productService.UpdateProductAsync(UpdateProduct, stream, token);
            if (updatedProduct == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update product.");
                return Page();
            }
            _ = SignalRPlay();
            return RedirectToPage("ProductManagement");
        }

        public async Task SignalRPlay()
        {
            if (_isSignalRRunnging)
            {
                return;
            }
            else if (!_isSignalRRunnging)
            {
                _isSignalRRunnging = true;
                try
                {
                    await Task.Delay(3000); // Simulate some delay for the SignalR connection
                }
                finally
                {
                    _isSignalRRunnging = false; // Reset the flag after the operation is complete
                }
                _ = _notificationService.NotifyProductUpdate(); // Notify product update
            }
        }

        #endregion
    }
}
