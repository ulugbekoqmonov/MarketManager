using ClosedXML.Excel;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.PaymentTypes.Reports
{
   
    public record AddPaymentTypeFromExcel(IFormFile ExcelFile) : IRequest<List<PaymentType>>;
    public class AddExpiredProductFromExcelHandler : IRequestHandler<AddPaymentTypeFromExcel, List<PaymentType>>
    {

        public async Task<List<PaymentType>> Handle(AddPaymentTypeFromExcel request, CancellationToken cancellationToken)
        {

            if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                throw new ArgumentNullException("File", "file is empty or null");

            var file = request.ExcelFile;
            List<PaymentType> result = new List<PaymentType>();
            using (var ms = new MemoryStream())
            {

                await file.CopyToAsync(ms, cancellationToken);
                using (var wb = new XLWorkbook(ms))
                {
                    var sheet1 = wb.Worksheet(1);
                    int startRow = 2;
                    for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                    {
                        var expiredProduct = new PaymentType
                        {
                            Id = Guid.Parse(sheet1.Cell(row, 1).GetString()),
                            Name = sheet1.Cell(row, 2).GetString(),
                            

                        };

                        result.Add(expiredProduct);
                    }


                }
            }
            return result;
        }
    }
}
