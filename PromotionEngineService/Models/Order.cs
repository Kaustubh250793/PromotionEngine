using System.Collections.Generic;

namespace PromotionEngineService.Models
{
    public class Order
    {
        public List<Product> Items { get; set; }

        public int TotalAmount { get; set; }
    }
}
