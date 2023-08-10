using Microsoft.AspNetCore.Mvc;
using MarketManager.Application.UseCases.CompanysData.Command.CreateCompanyData;
using MarketManager.Application.UseCases.CompanysData.Responce;
using MarketManager.Application.UseCases.CompanysData.Queries.GetAllCompanyData;
using MarketManager.Application.UseCases.CompanyData.Command.UpdateCompanyData;
using MarketManager.Application.UseCases.CompanysData.Command.DeleteCompanyData;

namespace MarketManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDataController : BaseApiController
    {
        [HttpPost("[action]")]
        public async ValueTask<Guid> CreateCompanyData([FromForm] CreateCompanyDataCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("[action]")]
        public async ValueTask<CompanyDataResponce> GetCompanyData(GetCompanyDataQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdateCompanyData([FromForm] UpdateCompanyDataCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeleteCompanyData(DeleteCompanyDataCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
