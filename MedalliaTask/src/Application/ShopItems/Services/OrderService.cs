using System.Collections.Generic;
using System.Linq;
using MedalliaTask.Application.Common.Interfaces;
using MedalliaTask.Domain.Entities;

namespace MedalliaTask.Application.Items.Services
{
    public class OrderService : IOrderService
    {
        public double CalculateTotal(Order order, IList<SpecialOffer> specialOffers)
        {
            if (order.OrderItems == null || order.OrderItems.Count == 0) return 0;

            var offersDictionary = specialOffers.GroupBy(x => x.ShopItem)
                .ToDictionary(x => x.Key.Id, x=>x.OrderBy(x => x.Amount).ToList());

            // regroup

            var orderItemsAggregated = order.OrderItems.GroupBy(x => x.Item)
                .ToDictionary(x => x.Key, x => x.Sum(x => x.Amount));
            
            double total = 0;
            foreach (var pair in orderItemsAggregated)
            {
                var itemId = pair.Key.Id;
                var value = pair.Value;

                if (offersDictionary.ContainsKey(itemId))
                {
                    var index = offersDictionary[itemId].Count-1;

                    while (index >= 0 && value > 0)
                    {
                        var offer = offersDictionary[itemId][index];
                        if (offer.Amount > value)
                        {
                            index--;
                        }
                        else
                        {
                            value -= offer.Amount;
                            total += offer.Price;
                        }
                    }

                }
                
                total += value * pair.Key.Price;
            }

            return total;
        }
    }
}