
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;
using ProductAndCategoryAPI.Service;

namespace ProductAndCategoryAPI.Controllers
{
    [Route("odata/OdataProduct")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // Ignore this controller in Swagger documentation
    public class OdataProductController : ODataController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OdataProductController(IProductService productService, IMapper mapper, IWebHostEnvironment env)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _env = env;
        }

        public async Task<IQueryable<ReadProductDTO>> GetAsync(ODataQueryOptions<Product> queryOptions)
        {
            // 1. Get IQueryable<Product> from the service
            IQueryable<Product> products = _productService.GetAllProductForOData();
            IQueryable<Product> results = (IQueryable<Product>)queryOptions.ApplyTo(products);

            //var uploads = Path.Combine(_env.WebRootPath, "img");
            //if (!Directory.Exists(uploads))
            //    Directory.CreateDirectory(uploads);

            //var filePath = Path.Combine(uploads, file.FileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

           
            //    Url = $"{Request.Scheme}://{Request.Host}/uploads/{file.FileName}"
            

            // 3. Project the results to ReadProductDTO
            IQueryable<ReadProductDTO> projectedResults = results.ProjectTo<ReadProductDTO>(_mapper.ConfigurationProvider);
            // 4. Return the projected results
            return projectedResults;
        }
    }

}
