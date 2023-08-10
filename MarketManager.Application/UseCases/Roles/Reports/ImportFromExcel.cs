using ClosedXML.Excel;
using MarketManager.Application.UseCases.Roles.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.Roles.Reports;
public record ImportFromExcel(IFormFile ExcelFile) : IRequest<List<RoleResponse>>;

public class ImportFromExcelHandler : IRequestHandler<ImportFromExcel, List<RoleResponse>>
{
    public async Task<List<RoleResponse>> Handle(ImportFromExcel request, CancellationToken cancellationToken)
    {
        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<RoleResponse> result = new List<RoleResponse>();
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var role = new RoleResponse
                    {
                        Id = Guid.NewGuid(),
                        Name = sheet1.Cell(row, 2).GetString(),
                    };

                    result.Add(role);
                }
            }
        }
        return result;
    }
}
