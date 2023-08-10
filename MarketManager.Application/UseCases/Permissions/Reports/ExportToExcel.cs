using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Permissions.ResponseModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketManager.Application.UseCases.Permissions.Reports;
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
        using (XLWorkbook wb = new XLWorkbook())
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
        var permissions = await _context.Permissions.ToListAsync(cancellationToken);
        DataTable dataTable = new("Permission");
        dataTable.Columns.AddRange(new DataColumn[] 
        {
            new DataColumn("Id"),
            new DataColumn("Name")
        });
        var mappingPermissions = _mapper.Map<List<PermissionResponse>>(permissions);
        if (mappingPermissions.Count > 0)
        {
            foreach (var item in mappingPermissions)
            {
                dataTable.Rows.Add(item.PermissionId, item.PermissionName);
            }
        }
        return dataTable;
    }
}
