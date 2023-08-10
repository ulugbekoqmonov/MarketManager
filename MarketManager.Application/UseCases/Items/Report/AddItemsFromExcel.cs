using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Items.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Item = MarketManager.Domain.Entities.Item;

namespace MarketManager.Application.UseCases.Items.Import.Export;

public record AddItemsFromExcel(IFormFile ExcelFile) : IRequest<List<ItemResponse>>;

public class AddItemsFromExcelHandler : IRequestHandler<AddItemsFromExcel, List<ItemResponse>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddItemsFromExcelHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ItemResponse>> Handle(AddItemsFromExcel request, CancellationToken cancellationToken)
    {
        if (request.ExcelFile == null || request.ExcelFile.Length == 0)
            throw new ArgumentNullException("File", "file is empty or null");

        var file = request.ExcelFile;
        List<Item> result = new();
        using (var ms = new MemoryStream())
        {

            await file.CopyToAsync(ms, cancellationToken);
            using (var wb = new XLWorkbook(ms))
            {
                var sheet1 = wb.Worksheet(1);
                int startRow = 2;
                for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                {
                    var Item = new Item()
                    {   

                        ProductId = Guid.Parse(sheet1.Cell(row, 2).GetString()),
                        OrderId = Guid.Parse(sheet1.Cell(row, 3).GetString()),
                        Amount = double.Parse(sheet1.Cell(row, 4).GetString()),
                        TotalPrice = double.Parse(sheet1.Cell(row, 5).GetString()),
                    };

                    result.Add(Item);
                }
            }
        }
        await _context.Items.AddRangeAsync(result);
        await _context.SaveChangesAsync();
        return _mapper.Map<List<ItemResponse>>(result);
    }
}
                        