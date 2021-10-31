using System;
using System.Collections.Generic;
using PromotionEngineService.Models;

namespace PromotionEngineService.Controller
{
    public class PromotionController
    {
        private readonly IEnumerable<SKU_Price> _sKU_Prices;
        private readonly List<Promotion> _promotions;

        public PromotionController(IEnumerable<SKU_Price> sKU_Prices, List<Promotion> promotions)
        {
            _sKU_Prices = sKU_Prices;
            _promotions = promotions;
        }

        public void CheckOutWithPromotion(Order order)
        {
        }
    }
}
