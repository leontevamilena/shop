using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDbLibrary.DTOs
{
    public class UpdateOrderDto
    {
        public DateOnly DeliveryDate { get; set; }
        public int StatusId { get; set; }
    }
}
