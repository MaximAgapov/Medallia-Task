using System.Collections.Generic;
using MedalliaTask.Application.Common.Interfaces;
using MedalliaTask.Application.Items.Services;
using MedalliaTask.Domain.Entities;
using NUnit.Framework;

namespace MedalliaTask.Application.UnitTests.Items.Services
{
    public class OrderServiceTests
    {
        private IOrderService _service;

        private ShopItem _apple;
        private ShopItem _banana;
        private ShopItem _oranges;
        private ShopItem _ananas;
        
        private double _delta = 0.00001;

        public OrderServiceTests()
        {
            _service = new OrderService();
            
            _apple = new ShopItem { Id = 1, Name = "Apple", Price = 0.50 }; 
            _banana = new ShopItem { Id = 2, Name = "Banana", Price = 0.70 }; 
            _oranges = new ShopItem { Id = 3, Name = "Oranges", Price = 0.45 }; 
            _ananas = new ShopItem {  Id = 4, Name = "Ananas", Price = 1.00 }; 
        }


        [Test]
        public void TotalShouldBeCalculatedPerItemIfNoSpecialOffer()
        {

            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Item = _apple, Amount = 2 },
                    new OrderItem { Item = _ananas, Amount = 2 },
                }
            };
            var result = _service.CalculateTotal(order, new List<SpecialOffer>());

            
            Assert.AreEqual(3, result);

        }
        
        [Test]
        public void ShouldCalculateUnordered()
        {

            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Item = _apple, Amount = 2 },
                    new OrderItem { Item = _ananas, Amount = 2 },
                    new OrderItem { Item = _apple, Amount = 2 },
                }
            };
            var result = _service.CalculateTotal(order, new List<SpecialOffer>());

            
            Assert.AreEqual(4, result);
        }
        
        [Test]
        public void ShouldCalculateWithOffer()
        {
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Item = _banana, Amount = 2 },
                    new OrderItem { Item = _ananas, Amount = 2 },
                    new OrderItem { Item = _banana, Amount = 2 },
                }
            };

            var offer = new SpecialOffer
            {
                Amount = 2,
                Price = 1,
                ShopItem = _banana,
            };
            var resultNoOffer = _service.CalculateTotal(order, new List<SpecialOffer>());
            var resultOffer = _service.CalculateTotal(order, new List<SpecialOffer>{offer});

            Assert.AreNotEqual(resultNoOffer, resultOffer);
            Assert.AreEqual(4, resultOffer, _delta);
            Assert.AreEqual(4.8, resultNoOffer, _delta);
        }
        
        [Test]
        public void OfferCanBeAppliedMaxTimes()
        {
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Item = _oranges, Amount = 7 },
                    new OrderItem { Item = _ananas, Amount = 2 },
                }
            };

            var offer = new SpecialOffer
            {
                Amount = 3,
                Price = 0.9,
                ShopItem = _oranges,
            };
            var resultOffer = _service.CalculateTotal(order, new List<SpecialOffer>{offer});
            Assert.AreEqual(4.25, resultOffer, _delta);
        }
        
        [Test]
        public void TakeBestOffer()
        {
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Item = _banana, Amount = 3 },
                    new OrderItem { Item = _ananas, Amount = 2 },
                    new OrderItem { Item = _banana, Amount = 3 },
                }
            };

            var offer = new SpecialOffer
            {
                Amount = 2,
                Price = 1,
                ShopItem = _banana,
            };
            
            var offerBest = new SpecialOffer
            {
                Amount = 5,
                Price = 1.5,
                ShopItem = _banana,
            };
            var resultOffer = _service.CalculateTotal(order, new List<SpecialOffer>{offer, offerBest});
            Assert.AreEqual(4.2, resultOffer, _delta);
        }
        
        [Test]
        public void TakeBestOfferAppliesAndOtherAsWellIfPossible()
        {
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Item = _banana, Amount = 8 },
                }
            };

            var offer = new SpecialOffer
            {
                Amount = 2,
                Price = 1,
                ShopItem = _banana,
            };
            
            var offerBest = new SpecialOffer
            {
                Amount = 5,
                Price = 1.5,
                ShopItem = _banana,
            };
            var resultOffer = _service.CalculateTotal(order, new List<SpecialOffer>{offer, offerBest});
            Assert.AreEqual(3.2, resultOffer, _delta);
        }
    }
}