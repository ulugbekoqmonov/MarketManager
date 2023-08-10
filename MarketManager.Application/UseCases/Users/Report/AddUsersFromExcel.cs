using ClosedXML.Excel;
using MarketManager.Application.UseCases.Users.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.Users.Report;
public record AddUsersFromExcel(IFormFile ExcelFile) : IRequest<List<UserResponse>>;
public class AddUsersFromExcelHandler : IRequestHandler<AddUsersFromExcel, List<UserResponse>>
{

    public async Task<List<UserResponse>> Handle(AddUsersFromExcel request, CancellationToken cancellationToken)
    {

        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<UserResponse> result = new List<UserResponse>();
        using (var ms = new MemoryStream())
        {

            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var user = new UserResponse
                    {
                        Id = Guid.NewGuid(),
                        FullName = sheet1.Cell(row, 2).GetString(),
                        Username = sheet1.Cell(row, 3).GetString(),
                        Phone = sheet1.Cell(row, 4).GetString(),
                       
                        
                    };

                    result.Add(user);
                }


            }
        }
        return result;
    }
}
