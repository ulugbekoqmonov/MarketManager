using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Items.Commands.CreateItem;
using MarketManager.Application.UseCases.Items.Commands.DeleteItem;
using MarketManager.Application.UseCases.Items.Commands.UpdateItem;
using MarketManager.Application.UseCases.Items.Import.Export;
using MarketManager.Application.UseCases.Items.Queries.GetAllItems;
using MarketManager.Application.UseCases.Items.Queries.GetItemById;
using MarketManager.Application.UseCases.Items.Queries.GetItemsPagination;
using MarketManager.Application.UseCases.Items.Response;
using MarketManager.Application.UseCases.Products.Queries.GetAllProductsWithPagination;
using MarketManager.Application.UseCases.Products.Response;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace MarketManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : BaseApiController
    {
        [HttpPost("[action]")]
        public async ValueTask<Guid> CreateItem(CreateItemCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("[action]")]
        public async Task<List<ItemResponse>> ImportExcelItems(IFormFile excelfile)
        {
            var result = await _mediator.Send(new AddItemsFromExcel(excelfile));
            return result;
        }

        [HttpGet("[action]")]
        public async ValueTask<ItemResponse> GetItemById(Guid Id)
        {
            return await _mediator.Send(new GetItemByIdQuery(Id));
        }

        [HttpGet("[action]")]
        public async ValueTask<IEnumerable<ItemResponse>> GetAllItems(int page = 1, int pageSize = 10)
        {
            IPagedList<ItemResponse> query = (await _mediator
               .Send(new GetAllItemsQuery()))
               .ToPagedList(page, pageSize);
            return query;
        }

        [HttpGet("[action]")]
        public async ValueTask<ActionResult<PaginatedList<ItemResponse>>> GetAllItemsPagination(
        [FromQuery] GetItemsPaginationQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<FileResult> ExportExcelItems(string fileName = "items")
        {
            var result = await _mediator.Send(new GetItemExcel { FileName = fileName });
            return File(result.FileContents, result.Option, result.FileName);
        } 

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdateItem(UpdateItemCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeleteItem(DeleteItemCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
