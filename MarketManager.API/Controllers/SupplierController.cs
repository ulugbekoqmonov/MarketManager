using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Clients.Reports;
using MarketManager.Application.UseCases.Suppliers.Commands.CreateSupplier;
using MarketManager.Application.UseCases.Suppliers.Commands.DeleteSupplier;
using MarketManager.Application.UseCases.Suppliers.Commands.UpdateSupplier;
using MarketManager.Application.UseCases.Suppliers.Filters;
using MarketManager.Application.UseCases.Suppliers.Queries.GetAllSuppliers;
using MarketManager.Application.UseCases.Suppliers.Queries.GetSupplierById;
using MarketManager.Application.UseCases.Suppliers.Report;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : BaseApiController
{

    [HttpGet("[action]")]
    public async Task<ActionResult<PaginatedList<GetAllSuppliersQueryResponse>>> FilterSuppliers(
        [FromQuery] SuppliersFilterQuery filter)
    {
        return await _mediator.Send(filter);
    }

    [HttpGet("[action]")]
    public async ValueTask<ActionResult<PaginatedList<GetAllSuppliersQueryResponse>>> GetAllSuppliers([FromQuery] GetAllSuppliersQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet("[action]")]
    public async ValueTask<GetSupplierByIdQueryRespоnse> GetSupplierById(Guid Id)
    {
        return await _mediator.Send(new GetSupplierByIdQuery(Id));
    }

    [HttpGet("[action]")]
    public async Task<FileResult> ExportExcelSuppliers(string fileName = "suppliers")
    {
        var result = await _mediator.Send(new SupplierExportExcel { FileName = fileName });
        return File(result.FileContents, result.Option, result.FileName);
    }

    [HttpPost("[action]")]
    public async Task<List<SuppliersResponseExcelReport>> ImportExcelSuppliers(IFormFile excelfile)
    {
        List<SuppliersResponseExcelReport> result = await _mediator.Send(new SupplierImportExcel(excelfile));
        return result;
    }

    [HttpPost("[action]")]
    public async ValueTask<Guid> CreateSupplier(CreateSupplierCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("[action]")]
    public async ValueTask<IActionResult> UpdateSupplier(UpdateSupplierCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("[action]")]
    public async ValueTask<IActionResult> DeleteSupplier(DeleteSupplierCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
