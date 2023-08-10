using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Roles.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketManager.Application.UseCases.Roles.Reports;
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
            var roleData = await CreateExcelAsync();
            var worksheet = wb.AddWorksheet(roleData, "Users");

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
        var roles = await _context.Roles.ToListAsync(cancellationToken);
        DataTable dt = new DataTable();
        dt.TableName = "Empdata";
        dt.Columns.Add("Code", typeof(Guid));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Permissions", typeof(string));

        var _list = _mapper.Map<List<RoleResponse>>(roles);
        if (_list.Count > 0)
        {
            _list.ForEach(item =>
            {
                dt.Rows.Add(item.Id, item.Name, string.Join(",", item.PermissionNames));

            });
        }

        return dt;
    }
}
