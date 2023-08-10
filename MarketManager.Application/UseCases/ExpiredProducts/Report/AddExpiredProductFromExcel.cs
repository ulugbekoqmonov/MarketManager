using ClosedXML.Excel;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.ExpiredProducts.Report
{

    public record AddExpiredProductFromExcel(IFormFile ExcelFile) : IRequest<List<ExpiredProduct>>;
    public class AddExpiredProductFromExcelHandler : IRequestHandler<AddExpiredProductFromExcel, List<ExpiredProduct>>
    {

        public async Task<List<ExpiredProduct>> Handle(AddExpiredProductFromExcel request, CancellationToken cancellationToken)
        {
          

            if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                throw new ArgumentNullException("File", "file is empty or null");

            var file = request.ExcelFile;
            List<ExpiredProduct> result = new List<ExpiredProduct>();
            using (var ms = new MemoryStream())
            {

                await file.CopyToAsync(ms, cancellationToken);
                using (var wb = new XLWorkbook(ms))
                {
                    var sheet1 = wb.Worksheet(1);
                    int startRow = 2;
                    for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                    {
                        var expiredProduct = new ExpiredProduct
                        {
                            Id = Guid.Parse(sheet1.Cell(row, 1).GetString()),
                            PackageId = Guid.Parse(sheet1.Cell(row, 2).GetString()),
                            Count = int.Parse(sheet1.Cell(row, 3).GetString()),

                        };

                        result.Add(expiredProduct);
                    }


                }
            }
            return result;
        }
    }

}
