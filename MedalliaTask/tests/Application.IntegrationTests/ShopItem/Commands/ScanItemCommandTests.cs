using System;
using System.Linq;
using System.Threading.Tasks;
using MedalliaTask.Application.Common.Exceptions;
using MedalliaTask.Application.Items.Commands.ScanItem;
using MedalliaTask.Application.Items.Commands.StartNewCommand;
using NUnit.Framework;

namespace MedalliaTask.Application.IntegrationTests.ShopItem.Commands
{
    using static Testing;

    public class ScanItemCommandTests : TestBase
    {
        
        
        [Test]
        public async Task ShouldThrowIfNoItem()
        {
            var command = new ScanItemCommand{Name= "unknown", Amount = 5};
            
            Assert.ThrowsAsync<ArgumentException>(async () => await SendAsync(command));
        }

        [Test]
        public async Task ShouldThrowIfNoActiveOrder()
        {
            var command = new ScanItemCommand{Name= "Apple", Amount = 5};
            
            Assert.ThrowsAsync<NoActiveOrderException>(async () => await SendAsync(command, context =>
            {
                var order = context.Orders.SingleOrDefault(x=>x.IsActive);
                order.IsActive = false;
                context.SaveChanges();
            }));
        }
        
        [Test]
        public async Task ShouldAddOrderItemToOrder()
        {
            var order = await SendAsync(new StartNewCommand());
            
            Assert.AreEqual(0, order.OrderItems.Count);
            
            var command = new ScanItemCommand{Name= "Apple", Amount = 5};
            
            var result = await SendAsync(command, needSeed: false);
            Assert.AreEqual(order.Id, result.Id);
            Assert.AreEqual(1, result.OrderItems.Count);
            Assert.AreEqual(command.Amount, result.OrderItems[0].Amount);
            Assert.AreEqual(command.Name, result.OrderItems[0].Item.Name);
        }
    }
}