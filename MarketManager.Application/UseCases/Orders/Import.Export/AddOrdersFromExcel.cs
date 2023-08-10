using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Orders.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Order = MarketManager.Domain.Entities.Order;

namespace MarketManager.Application.UseCases.Orders.Import.Export;

public record AddOrdersFromExcel(IFormFile ExcelFile) : IRequest<List<OrderResponse>>;

public class AddOrdersFromExcelHandler : IRequestHandler<AddOrdersFromExcel, List<OrderResponse>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddOrdersFromExcelHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderResponse>> Handle(AddOrdersFromExcel request, CancellationToken cancellationToken)
    {
        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<Order> result = new();
        using (var ms = new MemoryStream())
        {

            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var order = new Order()
                    {
                        //  Id = Guid.Parse(sheet1.Cell(row, 1).GetString()),
                       // TotalPrice = double.Parse(sheet1.Cell(row, 1).GetString()),
                        CashbackSum = double.Parse(sheet1.Cell(row, 4).GetString()),
                        TotalPriceBeforeCashback = double.Parse(sheet1.Cell(row, 3).GetString()),
                        ClientId = Guid.Parse(sheet1.Cell(row, 5).GetString()),

                    };

                    result.Add(order);
                }


            }
        }
        await _context.Orders.AddRangeAsync(result);
        await _context.SaveChangesAsync();
        return _mapper.Map<List<OrderResponse>>(result);



    }
}

