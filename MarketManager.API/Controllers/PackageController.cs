using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Packages.Commands.CreatePackage;
using MarketManager.Application.UseCases.Packages.Commands.UpdatePackage;
using MarketManager.Application.UseCases.Packages.Queries.GetAllPackages;
using MarketManager.Application.UseCases.Packages.Queries.GetPackageById;
using MarketManager.Application.UseCases.Packages.Queries.GetPackagesPagination;
using MarketManager.Application.UseCases.Packages.Reports;
using MarketManager.Application.UseCases.Packages.Response;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : BaseApiController
    {
        [HttpPost("[action]")]
        public async ValueTask<Guid> CreatePackage(CreatePackageCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("[action]")]
        public async Task<List<PackageResponse>> ImportExcelPackages(IFormFile excelfile)
        {
            var result = await _mediator.Send(new AddPackagesFromExcel(excelfile));
            return result;
        }

        [HttpGet("[action]")]
        public async ValueTask<PackageResponse> GetPackageById(Guid Id)
        {
            return await _mediator.Send(new GetPackageByIdQuery(Id));
        }

        [HttpGet("[action]")]
        public async ValueTask<IEnumerable<PackageResponse>> GetAllPackages()
        {
            return await _mediator.Send(new GetAllPackagesQuery());
        }

        [HttpGet("[action]")]
        public async ValueTask<ActionResult<PaginatedList<PackageResponse>>> GetAllPackagesPagination(
            [FromQuery] GetPackagesPaginationQuery query)
        {
            return await _mediator.Send(query);
        }


        [HttpGet("[action]")]
        public async Task<FileResult> ExportExcelPackages(string fileName = "packages")
        {
            var result = await _mediator.Send(new GetPackagesExcel { FileName = fileName });
            return File(result.FileContents, result.Option, result.FileName);
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdatePackage(UpdatePackageCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeletePackage(UpdatePackageCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
