using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedalliaTask.Application.Common.Exceptions;
using MedalliaTask.Application.Common.Interfaces;
using MedalliaTask.Domain.Entities;
using MediatR;

namespace MedalliaTask.Application.Items.Commands.ScanItem
{
    public class ScanItemCommand: IRequest<Order>
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }

    public class AddItemCommandHandler: IRequestHandler<ScanItemCommand, Order>
    {
        private readonly IApplicationDbContext _context;

        public AddItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Order> Handle(ScanItemCommand request, CancellationToken cancellationToken)
        {
            var shopItem = _context.ShopItems.SingleOrDefault(x => x.Name == request.Name);
            if (shopItem == null)
                throw new ArgumentException($"There is no Item with name {request.Name}");

            var order = _context.Orders.SingleOrDefault(x => x.IsActive);
            if (order == null)
                throw new NoActiveOrderException();
                
            order.OrderItems.Add(new OrderItem{Amount = request.Amount, Item = shopItem});
            await _context.SaveChangesAsync(cancellationToken);
            return order;
        }
    }
}