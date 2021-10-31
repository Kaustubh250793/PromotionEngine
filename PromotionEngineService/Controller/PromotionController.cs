using System.Collections.Generic;
using System.Linq;
using PromotionEngineService.Models;

namespace PromotionEngineService.Controller
{
    public class PromotionController
    {
        private readonly IEnumerable<SKU_Price> _priceList;
        private readonly IEnumerable<Promotion> _promotions;

        public PromotionController(IEnumerable<SKU_Price> priceList, IEnumerable<Promotion> promotions)
        {
            _priceList = priceList;
            _promotions = promotions;
        }

        public int CheckOutWithPromotion(Order order)
        {
            foreach (var promotion in _promotions)
            {
                order.TotalAmount += CalculatePromotionPrice(order, promotion);
            }

            order.TotalAmount += CalculateRegularPrice(order);

            return order.TotalAmount;
        }

        private int CalculateRegularPrice(Order order)
        {
            int price = 0;
            foreach (var item in order.Items)
            {
                price += GetPriceFromPriceList(item.SKU_Id) * item.Quantity;
            }

            return price;
        }

        private int CalculatePromotionPrice(Order order, Promotion promotion)
        {
            var promotionItemsCountDict = new Dictionary<char, int>();

            var promotionItems = promotion.Items;
            int totalPrice = 0;

            if (promotion.Items.All(x => order.Items.FirstOrDefault(p => p.SKU_Id == x.SKU_Id)?.Quantity >= x.Quantity))
            {
                foreach (var item in promotionItems)
                {
                    var promotionItemQuantity = order.Items.FirstOrDefault(x => x.SKU_Id == item.SKU_Id).Quantity;

                    promotionItemsCountDict.Add(item.SKU_Id, promotionItemQuantity);
                }

                while (order.Items.Where(p => promotionItems.Any(x => x.SKU_Id == p.SKU_Id)).Sum(q => q.Quantity)
                >= promotionItems.Sum(x => x.Quantity))
                {
                    totalPrice += promotion.TotalAmount;

                    foreach (var item in promotionItems)
                    {
                        promotionItemsCountDict[item.SKU_Id] -= item.Quantity;
                    }
                }
                foreach (var key in promotionItemsCountDict.Keys)
                {
                    var reomve = order.Items.FirstOrDefault(s => s.SKU_Id == key);
                    order.Items.Remove(reomve);
                }

                promotionItemsCountDict.Keys.Select(x => order.Items.Remove(order.Items.FirstOrDefault(s => s.SKU_Id == x)));

                foreach (var item in promotionItemsCountDict.Where(x => x.Value > 0))
                {
                    totalPrice += GetPriceFromPriceList(item.Key) * item.Value;
                }

                return totalPrice;
            }

            return 0;
        }

        public int GetPriceFromPriceList(char id)
        {
            return _priceList.FirstOrDefault(x => x.SKU_Id == id)?.UnitPrice ?? 0;
        }
    }
}
