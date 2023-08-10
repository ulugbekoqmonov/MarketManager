using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Permissions.ResponseModels;
using MarketManager.Application.UseCases.ProductsType.Commands.DeleteProductType;
using MarketManager.Application.UseCases.ProductsType.Commands.UpdateProductType;
using MarketManager.Application.UseCases.ProductTypes.Commands.CreateProductsType;
using MarketManager.Application.UseCases.ProductTypes.Queries.GetAllProductTypes;
using MarketManager.Application.UseCases.ProductTypes.Queries.GetByIdProductTypeQuery;
using MarketManager.Application.UseCases.ProductTypes.Reports;
using MarketManager.Application.UseCases.ProductTypes.Response;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductTypeController : BaseApiController
{
    [HttpGet("[action]")]
    public async ValueTask<PaginatedList<ProductTypeResponce>> GetAllProductTypes([FromQuery] GetAllProductTypesQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet("[action]")]
    public async ValueTask<ProductTypeResponce> GetProductTypeById(Guid id)
    {
        return await _mediator.Send(new GetByIdProductTypeQuery(id));
    }

    [HttpPost("[action]")]
    public async ValueTask<Guid> CreateProductType([FromForm] CreateProductTypeCommand command)
    {
        return await _mediator.Send(command);
    }
    [HttpPut("[action]")]
    public async ValueTask<IActionResult> UpdateProductType([FromForm] UpdateProductTypeCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("[action]")]
    public async ValueTask<IActionResult> DeleteProductType([FromForm] DeleteProductTypeCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("[action]")]
    public async Task<FileResult> ExportToExcel(string fileName = "ProductType")
    {
        var result = await _mediator.Send(new ExportToExcel { FileName = fileName });
        return File(result.FileContents, result.Option, result.FileName);
    }

    [HttpPost("[action]")]
    public async Task<List<ProductTypeResponce>> ImportFromExcel(IFormFile excelfile)
    {
        var result = await _mediator.Send(new ImportFromExcel(excelfile));
        return result;
    }
}
