using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.ProductTypes.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketManager.Application.UseCases.ProductTypes.Reports;
public record ExportToExcel : IRequest<ExcelReportResponse>
{
    public string FileName { get; set; }
}

public class ExportToExcelHandler : IRequestHandler<ExportToExcel, ExcelReportResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ExportToExcelHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExcelReportResponse> Handle(ExportToExcel request, CancellationToken cancellationToken)
    {
        using(XLWorkbook wb = new XLWorkbook())
        {
            var productType = await CreateExcelAsync(cancellationToken);
            var worksheet = wb.AddWorksheet(productType, "ProductTypes");

            worksheet.Columns().AdjustToContents(20.0, 80.0);
            worksheet.RowHeight = 20;
            using (MemoryStream ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                return new ExcelReportResponse(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");

            }
        }
    }
    private async Task<DataTable> CreateExcelAsync(CancellationToken cancellationToken = default)
    {
        var productTypes = await _context.ProductTypes.ToListAsync(cancellationToken);
        DataTable dataTable = new("ProductType");
        dataTable.Columns.AddRange(new DataColumn[]
        {
            new DataColumn("Id"),
            new DataColumn("Name")
        });
        var mappingProductTypes = _mapper.Map<List<ProductTypeResponce>>(productTypes);
        if (mappingProductTypes.Count > 0)
        {
            foreach (var item in mappingProductTypes)
            {
                dataTable.Rows.Add(item.Id, item.Name);
            }
        }
        return dataTable;
    }
} 
