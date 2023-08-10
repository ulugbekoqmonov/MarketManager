using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace MarketManager.Application.UseCases.Clients.Reports;
public record AddClientFromExcel(IFormFile ExcelFile) : IRequest<List<ClientsResponseExcelReport>>;
public class AddClientFromExcelHandler : IRequestHandler<AddClientFromExcel, List<ClientsResponseExcelReport>>
{
    public async Task<List<ClientsResponseExcelReport>> Handle(AddClientFromExcel request, CancellationToken cancellationToken)
    {

        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<ClientsResponseExcelReport> result = new List<ClientsResponseExcelReport>();
        using (var ms = new MemoryStream())
        {

            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var Client = new ClientsResponseExcelReport
                    {
                        Id = Guid.Parse(sheet1.Cell(row, 1).GetString()),
                        CashbackSum = double.Parse(sheet1.Cell(row, 2).GetString()),
                        PhoneNumber = sheet1.Cell(row, 3).GetString()

                    };

                    result.Add(Client);
                }
            }
        }
        return result;
    }
}
