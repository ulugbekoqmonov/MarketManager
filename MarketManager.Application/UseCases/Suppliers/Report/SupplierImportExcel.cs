using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.Suppliers.Report;

public record SupplierImportExcel(IFormFile ExcelFile) : IRequest<List<SuppliersResponseExcelReport>>;

public class AddSupplierFromExcelHandelr : IRequestHandler<SupplierImportExcel, List<SuppliersResponseExcelReport>>
{
    public async Task<List<SuppliersResponseExcelReport>> Handle(SupplierImportExcel request, CancellationToken cancellationToken)
    {

        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<SuppliersResponseExcelReport> result = new();
        using (var ms = new MemoryStream())
        {

            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var Supplier = new SuppliersResponseExcelReport
                    {
                        Id = Guid.Parse(sheet1.Cell(row, 1).GetString()),
                        Name = sheet1.Cell(row, 2).GetString(),
                        Phone = sheet1.Cell(row, 3).GetString()

                    };

                    result.Add(Supplier);
                }
            }
        }
        return result;
    }
}
