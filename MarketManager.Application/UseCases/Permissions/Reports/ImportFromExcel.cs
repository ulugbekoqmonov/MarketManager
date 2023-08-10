using ClosedXML.Excel;
using MarketManager.Application.UseCases.Permissions.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.Permissions.Reports;
public record ImportFromExcel(IFormFile ExcelFile) : IRequest<List<PermissionResponse>>;

public class ImportFromExcelHandler : IRequestHandler<ImportFromExcel, List<PermissionResponse>>
{
    public async Task<List<PermissionResponse>> Handle(ImportFromExcel request, CancellationToken cancellationToken)
    {
        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<PermissionResponse> result = new List<PermissionResponse>();
        using (var ms = new MemoryStream())
        {

            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var permission = new PermissionResponse
                    {
                        PermissionId = Guid.NewGuid(),
                        PermissionName = sheet1.Cell(row, 2).GetString()
                    };

                    result.Add(permission);
                }
            }
        }
        return result;
    }
}
