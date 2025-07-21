
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
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OdataProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [EnableQuery]
        public IActionResult Get(ODataQueryOptions<Product> queryOptions)
        {
            IQueryable<Product> products = _productService.GetAllProductForOData();
            IQueryable<Product> results = (IQueryable<Product>)queryOptions.ApplyTo(products);
            IQueryable<ReadProductDTO> projectedResults = results.ProjectTo<ReadProductDTO>(_mapper.ConfigurationProvider);
            return Ok(projectedResults);
        }

    }

}
