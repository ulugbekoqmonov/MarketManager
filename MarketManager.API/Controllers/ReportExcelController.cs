using MarketManager.Application.UseCases.Permissions.Reports;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportExcelController : BaseApiController
{
    [HttpGet("[action]")]
    public async Task<FileResult> ReportToExcelGeneric(string name)
    {
        var result = await _mediator.Send(new GenericReportToExcel() { EndpoinName = name });
        return File(result.FileContents, result.Option, result.FileName);
    }
}
