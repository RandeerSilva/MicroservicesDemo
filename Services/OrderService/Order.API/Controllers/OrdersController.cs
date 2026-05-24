using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController(CreateOrderHandler handler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var id = await handler.Handle();
            return Ok(id);
        }
    }
}
