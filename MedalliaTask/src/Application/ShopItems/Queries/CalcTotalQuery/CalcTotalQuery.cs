using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedalliaTask.Application.Common.Exceptions;
using MedalliaTask.Application.Common.Interfaces;
using MediatR;

namespace MedalliaTask.Application.Items.Queries.GetTotal
{
    public class CalcTotalQuery : IRequest<double>
    {
    }

    public class
        GetTotalQueryHandler : IRequestHandler<CalcTotalQuery, double>
    {
        private readonly IApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public GetTotalQueryHandler(IApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<double> Handle(CalcTotalQuery request,
            CancellationToken cancellationToken)
        {
            var offers = _context.SpecialOffer.ToList();
            var order = _context.Orders.SingleOrDefault(x => x.IsActive);
            if (order == null)
                throw new NoActiveOrderException();
            return _orderService.CalculateTotal(order, offers);
        }
    }
}