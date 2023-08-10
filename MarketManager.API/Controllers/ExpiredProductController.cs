using MarketManager.Application.Common.Abstraction;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.ExpiredProducts.Command.CreateExpiredProduct;
using MarketManager.Application.UseCases.ExpiredProducts.Command.DeleteExpiredProduct;
using MarketManager.Application.UseCases.ExpiredProducts.Command.UpdateExpiredProduct;
using MarketManager.Application.UseCases.ExpiredProducts.Filters;
using MarketManager.Application.UseCases.ExpiredProducts.Queries;
using MarketManager.Application.UseCases.ExpiredProducts.Queries.GetAllExpiredProducts;
using MarketManager.Application.UseCases.ExpiredProducts.Report;
using MarketManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpiredProductController : BaseApiController
    {
        [HttpPost("[action]")]
        public async ValueTask<Guid> CreateExpiredProduct(CreateExpiredProductCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("[action]")]
        public async ValueTask<PaginatedList<ExpiredProductResponce>> GelAllExpiredProduct([FromQuery] GetAllExpiredProductsQuery query)
        {
            return await _mediator.Send(query);
        }
       

        [HttpGet("[action]")]
        public async ValueTask<ExpiredProductResponce> GetByIdExpiredProduct(Guid Id)
        {
            return await _mediator.Send(new GetByIdExpiredProductsQuery(Id));
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdateExpiredProduct(UpdateExpiredProductCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeleteExpiredProduct(DeleteExpiredProductCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("[action]")]
        public async ValueTask<PaginatedList<ExpiredProductResponce>> ExpiredProductFilter([FromQuery] GetExpiredProductFilterQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<FileResult> ExportExcelExpiredProduct(string fileName = "expiredProduct")
        {
            var result = await _mediator.Send(new GetExpiredProductFromExcel { FileName = fileName });
            return File(result.FileContents, result.Option, result.FileName);
        }


        [HttpPost("[action]")]
        public async Task<List<ExpiredProduct>> ImportExcelExpiredProduct(IFormFile excelfile)
        {
            List<ExpiredProduct> result = await _mediator.Send(new AddExpiredProductFromExcel(excelfile));
            return result;
        }
       

    }
}
