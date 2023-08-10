using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Orders.Commands.CreateOrder;
using MarketManager.Application.UseCases.Orders.Commands.DeleteOrder;
using MarketManager.Application.UseCases.Orders.Commands.UpdateOrder;
using MarketManager.Application.UseCases.Orders.Import.Export;
using MarketManager.Application.UseCases.Orders.Queries.GetAllOrders;
using MarketManager.Application.UseCases.Orders.Queries.GetOrder;
using MarketManager.Application.UseCases.Orders.Queries.GetOrdersWithFilter;
using MarketManager.Application.UseCases.Orders.Response;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace MarketManager.API.Controllers
{
    public class OrderController : BaseApiController
    {
        [HttpGet("[action]")]
        public async ValueTask<IEnumerable<OrderResponse>> GetAllOrders(int page = 1)
        {
            IPagedList<OrderResponse> query = (await _mediator
               .Send(new GetAllOrderQuery()))
               .ToPagedList(page, 10);
            return query;
        }

        [HttpGet("[action]")]
        public async ValueTask<OrderResponse> GetOrderById(Guid Id)
        {
            return await _mediator.Send(new GetOrderQuery(Id));
        }

        [HttpPost("[action]")]
        public async ValueTask<Guid> CreateOrder(CreateOrderCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeleteOrder(DeleteOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<FileResult> ExportExcelOrders(string fileName = "orders")
        {
            var result = await _mediator.Send(new GetOrderExcelQuery { FileName = fileName });
            return File(result.FileContents, result.Option, result.FileName);
        }


        [HttpPost("[action]")]
        public async Task<List<OrderResponse>> ImportExcelOrders(IFormFile excelfile)
        {
            var result = await _mediator.Send(new AddOrdersFromExcel(excelfile));
            return result;
        }

        [HttpGet("[action]")]
        public async Task<FileResult> ExportPdfOrders(string fileName = "orders")
        {
            var result = await _mediator.Send(new GetOrderPDF(fileName));
            return File(result.FileContents, result.Options, result.FileName);
        }

        [HttpPost("[action]")]
        public async Task<List<OrderResponse>> ImportPdfOrders(IFormFile excelfile)
        {
            var result = await _mediator.Send(new AddOrdersFromPDF(excelfile));
            return result;
        }

        [HttpGet("[action]")]
        public async ValueTask<PaginatedList<OrderResponse>> GetFilteredOrders([FromQuery] FilterOrderQuery query)
          => await _mediator.Send(query);

    }
}