using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedalliaTask.Application.Common.Interfaces;
using MedalliaTask.Domain.Entities;
using MediatR;

namespace MedalliaTask.Application.Items.Commands.StartNewCommand
{
    public class StartNewCommand: IRequest<Order>
    {
        
    }

    public class StartNewCommandHandler: IRequestHandler<StartNewCommand, Order>
    {
        private readonly IApplicationDbContext _context;

        public StartNewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Order> Handle(StartNewCommand request, CancellationToken cancellationToken)
        {
            var activeOrders = _context.Orders.Where(x => x.IsActive).ToList();
            activeOrders.ForEach(x => x.IsActive = false);
            var newOrder = new Order();
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync(cancellationToken);
            return newOrder;
        }
    }
}