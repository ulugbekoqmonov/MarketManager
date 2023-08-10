using ClosedXML.Excel;
using MarketManager.Application.UseCases.ProductTypes.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.ProductTypes.Reports;
public record ImportFromExcel(IFormFile ExcelFile) : IRequest<List<ProductTypeResponce>>;

public class ImportFromExcelHandler : IRequestHandler<ImportFromExcel, List<ProductTypeResponce>>
{

    public async Task<List<ProductTypeResponce>> Handle(ImportFromExcel request, CancellationToken cancellationToken)
    {
        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<ProductTypeResponce> result = new List<ProductTypeResponce>();
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var productType = new ProductTypeResponce
                    {
                        Id = Guid.NewGuid(),
                        Name = sheet1.Cell(row, 2).GetString(),
                        Picture = sheet1.Cell(row, 3).GetString()
                    };
                    result.Add(productType);
                }
            }
        }
        return result;
    }
}
