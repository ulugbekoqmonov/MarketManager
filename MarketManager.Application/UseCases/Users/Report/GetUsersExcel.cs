using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Users.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Users.Report;
public class GetUsersExcel : IRequest<ExcelReportResponse>
{
    public string FileName { get; set; }
}
public class GetUsersExcelHandler : IRequestHandler<GetUsersExcel, ExcelReportResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersExcelHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }

    public async Task<ExcelReportResponse> Handle(GetUsersExcel request, CancellationToken cancellationToken)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            var userData = await GetUsersAsync();
            var sheet1 = wb.AddWorksheet(userData, "Users");

            sheet1.Columns().AdjustToContents(20.0, 80.0);
            sheet1.RowHeight = 20;
            using (MemoryStream ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                return new ExcelReportResponse(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");

            }
        }
    }

    private async Task<DataTable> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var allUser = await _context.Users.ToListAsync(cancellationToken);
        DataTable dt = new DataTable();
        dt.TableName = "Empdata";
        dt.Columns.Add("Code", typeof(Guid));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Username", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Roles",typeof(string));
        dt.Columns.Add("Created date",typeof(DateTime));
        dt.Columns.Add("Created by",typeof(string));
        dt.Columns.Add("Modified date", typeof(DateTime));
        dt.Columns.Add("Modified by", typeof(string));

        var _list = _mapper.Map<List<UserResponse>>(allUser);
        if (_list.Count > 0)
        {
            _list.ForEach(item =>
            {
                dt.Rows.Add(item.Id, item.FullName, item.Username, item.Phone,string.Join(",",item.RoleNames),item.CreatedDate,item.CreatedBy,item.ModifyDate,item.ModifyBy);

            });
        }

        return dt;
    }

}



