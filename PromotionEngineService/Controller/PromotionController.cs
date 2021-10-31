using System.Collections.Generic;
using PromotionEngineService.Models;

namespace PromotionEngineService.Controller
{
    public class PromotionController
    {
        private readonly IEnumerable<SKU_Price> _sKU_Prices;
        private readonly IEnumerable<Product> _promotions;

        public PromotionController(IEnumerable<SKU_Price> sKU_Prices, IEnumerable<Product> promotions)
        {
            _sKU_Prices = sKU_Prices;
            _promotions = promotions;
        }
    }
}
