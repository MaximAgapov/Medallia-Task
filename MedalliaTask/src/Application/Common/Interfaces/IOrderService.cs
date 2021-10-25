using System.Collections.Generic;
using MedalliaTask.Domain.Entities;

namespace MedalliaTask.Application.Common.Interfaces
{
    public interface IOrderService
    {
        double CalculateTotal(Order order, IList<SpecialOffer> specialOffers);
    }
}