using System.Collections.Generic;
using NUnit.Framework;
using PromotionEngineService.Controller;
using PromotionEngineService.Models;

namespace PromotionEngineService.UnitTests
{
    public class PromotionControllerUnitTests
    {
        private PromotionController _controller;

        [SetUp]
        public void SetUp()
        {
            var priceList = new List<SKU_Price>
          {
                new SKU_Price { SKU_Id = 'A', UnitPrice = 50 },
                new SKU_Price { SKU_Id = 'B', UnitPrice = 30 },
                new SKU_Price { SKU_Id = 'C', UnitPrice = 20 },
                new SKU_Price { SKU_Id = 'D', UnitPrice = 15 }
          };

            var promotions = new List<Promotion>
            {
                // Promotion 1
                new Promotion()
                {
                    Items = new List<Product>()
                    {
                        new Product() { SKU_Id = 'A', Quantity = 3 }
                    },
                    TotalAmount = 130
                },

                // Promotion 2
                new Promotion()
                {
                    Items = new List<Product>()
                    {
                        new Product() { SKU_Id = 'B', Quantity = 2 }
                    },
                    TotalAmount = 45
                },

                // Promotion 3
                new Promotion()
                {
                    Items = new List<Product>()
                    {
                        new Product() {SKU_Id = 'C', Quantity = 1},
                        new Product() {SKU_Id = 'D', Quantity = 1}
                    },
                    TotalAmount = 30
                },

                // Promotion for item A - Check for mutual exclusiveness
                new Promotion()
                {
                    Items = new List<Product>()
                    {
                        new Product() { SKU_Id = 'A', Quantity = 2 }
                    },
                    TotalAmount = 110
                },
            };


            _controller = new PromotionController(priceList, promotions);
        }


        /// <summary>
        /// Scenario A
        /// 1* A = 50
        /// 1* B = 30
        /// 1* C = 20
        /// </summary>
        [Test]
        public void Test_Scenario_A()
        {
            // Arrange
            var order =
              new Order
              {
                  Items = new List<Product>
                {
                    new Product { SKU_Id = 'A', Quantity = 1 },
                    new Product { SKU_Id = 'B', Quantity = 1 },
                    new Product { SKU_Id = 'C', Quantity = 1 }
                }
              };

            // Act
            _controller.CheckOutWithPromotion(order);

            // Assert
            Assert.IsTrue(order.TotalAmount == 100);
        }

        /// <summary>
        /// Scenario B
        /// 5 * A = 130 + 2*50
        /// 5 * B = 45 + 45 + 30
        /// 1 * C = 20
        //Total = 370 
        /// </summary>
        [Test]
        public void Test_Scenario_B()
        {
            // Arrange
            var order =
              new Order
              {
                  Items = new List<Product>
                {
                    new Product { SKU_Id = 'A', Quantity = 5 },
                    new Product { SKU_Id = 'B', Quantity = 5 },
                    new Product { SKU_Id = 'C', Quantity = 1 }
                }
              };

            // Act
            _controller.CheckOutWithPromotion(order);

            // Assert
            Assert.IsTrue(order.TotalAmount == 370);
        }

        /// <summary>
        /// Scenario C
        /// 3* A = 130
        /// 5* B = 45 + 45 + 1 * 30
        /// 1* C = -
        /// 1* D = 30
        /// </summary>
        [Test]
        public void Test_Scenario_C()
        {
            // Arrange
            var order =
              new Order
              {
                  Items = new List<Product>
                {
                    new Product { SKU_Id = 'A', Quantity = 3 },
                    new Product { SKU_Id = 'B', Quantity = 5 },
                    new Product { SKU_Id = 'C', Quantity = 1 },
                    new Product { SKU_Id = 'D', Quantity = 1 }}
              };

            // Act
            _controller.CheckOutWithPromotion(order);

            // Assert
            Assert.IsTrue(order.TotalAmount == 280);
        }
    }
}
