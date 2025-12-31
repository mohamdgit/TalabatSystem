using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Dtos.BasketDtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicutreUrl { get; set; }
        [Range(1,Double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, 100)]
        public decimal Quantity { get; set; }
    }
}
