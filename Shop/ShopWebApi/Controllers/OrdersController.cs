using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopDbLibrary.Contexts;
using ShopDbLibrary.DTOs;
using ShopDbLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public OrdersController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders/user/{login}
        [Authorize] //метод, дающий доступ только после авторизации
        [HttpGet("user/{login}")]
        //получение списка заказов по логину
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserLogin(string login)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.CountProducts)
                .ThenInclude(so => so.Product)
                .Where(o => o.User.Login == login)
                .ToListAsync();

            if (orders is not null)
            {
                return Ok(orders);
            }
            return NotFound();
        }

        // PUT: api/Orders/{orderId}
        [Authorize(Roles = "admin, manager")]
        [HttpPut("{OrderId}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] UpdateOrderDto updatedOrder)
        {
            // поиск заказа по ID
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
                return NotFound();

            // обновление даты доставки, если передана и отличается от текущей
            if (updatedOrder.DeliveryDate != default && updatedOrder.DeliveryDate != order.DeliveryDate)
            {
                order.DeliveryDate = updatedOrder.DeliveryDate;
            }

            // обновление статуса (через StatusId), если передан и не равен 0
            if (updatedOrder.StatusId != 0 && updatedOrder.StatusId != order.StatusId)
            {
                // проверка, существует ли такой статус
                var statusExists = await _context.Statuses.AnyAsync(s => s.StatusId == updatedOrder.StatusId);
                if (!statusExists)
                    return BadRequest();

                order.StatusId = updatedOrder.StatusId;
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
