using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.ExpiredProducts.Report;
using MarketManager.Application.UseCases.PaymentTypes.Commands.CreatePaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Commands.DeletePaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Queries.GetAllPaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Queries.GetByIdPaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Reports;
using MarketManager.Application.UseCases.PaymentTypes.Responce;
using MarketManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers
{

    public class PaymentTypeController : BaseApiController
    {
        [HttpGet("[action]")]
        public async ValueTask<PaginatedList<GetPaymentTypeQueryResponse>> GetAllPaymentTypes([FromQuery]GetAllPaymentTypeQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async ValueTask<GetPaymentTypeQueryResponse> GetPaymentTypeById(Guid id)
        {
            return await _mediator.Send(new GetByIdPaymentTypeQuery() { Id = id });
        }

        [HttpPost("[action]")]
        public async ValueTask<Guid> CreatePaymentType(CreatePaymentTypeCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdatePaymentType(UpdatePaymentTypeCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeletePaymentType(DeletePaymentTypeCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<FileResult> ExportExcelPaymentType(string fileName = "expiredProduct")
        {
            var result = await _mediator.Send(new GetPaymentTypeFromExcel { FileName = fileName });
            return File(result.FileContents, result.Option, result.FileName);
        }


        [HttpPost("[action]")]
        public async Task<List<PaymentType>> ImportExcelPaymentType(IFormFile excelfile)
        {
            List<PaymentType> result = await _mediator.Send(new AddPaymentTypeFromExcel(excelfile));
            return result;
        }

    }
}
