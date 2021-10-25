using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedalliaTask.Application.Items.Commands.ScanItem;
using MedalliaTask.Application.Items.Commands.StartNewCommand;
using MedalliaTask.Application.Items.Queries.GetTotal;

namespace MedalliaTask.WebUI.Controllers
{
    public class OrderController : ApiControllerBase
    {
                
        [HttpPost]
        public async Task<ActionResult> StartNew()
        {
            await Mediator.Send(new StartNewCommand());
            return new AcceptedResult();
        }
        
        
        [HttpPost]
        public async Task<ActionResult> AddItemToOrder(ScanItemCommand command)
        {
            await Mediator.Send(command);
            return new AcceptedResult();
        }
        
        [HttpGet]
        public async Task<double> GetTotal()
        {
            return await Mediator.Send(new CalcTotalQuery());
        }
    }
}