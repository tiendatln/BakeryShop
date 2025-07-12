using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OrderAPI.DTOs;
using OrderAPI.Models;
using OrderAPI.Services.Interfaces;


namespace OrderAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ODataOrdersController : ODataController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ODataOrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // GET: odata/ODataOrders
        /*[EnableQuery]
        public IQueryable<Order> Get()
        {
             var userId = HttpContext.User.FindFirst(
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userId == null)
                return Enumerable.Empty<Order>().AsQueryable();

            return _orderService.GetOrderByUserIdQueryable(int.Parse(userId));
        }*/

        [EnableQuery]
        public IQueryable<Order> Get()
        {
           
            return _orderService.GetAllOrderQueryable();
        }
    } 
}
