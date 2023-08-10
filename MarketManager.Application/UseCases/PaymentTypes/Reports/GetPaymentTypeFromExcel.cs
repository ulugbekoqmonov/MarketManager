using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketManager.Application.UseCases.PaymentTypes.Reports
{

    public class GetPaymentTypeFromExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }
    public class GetExpiredProductFromExcelHandler : IRequestHandler<GetPaymentTypeFromExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetExpiredProductFromExcelHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetPaymentTypeFromExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var userData = await GetUsersAsync();
                var sheet1 = wb.AddWorksheet(userData, "expiredProduct");


                sheet1.Column(1).Style.Font.FontColor = XLColor.Red;

                sheet1.Columns(2, 3).Style.Font.FontColor = XLColor.Blue;

                sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;

                sheet1.Row(1).Style.Font.FontColor = XLColor.White;

                sheet1.Row(1).Style.Font.Bold = true;
                sheet1.Row(1).Style.Font.Shadow = true;
                sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                sheet1.Row(1).Style.Font.Italic = true;

                sheet1.RowHeight = 20;
                sheet1.Column(1).Width = 38;
                sheet1.Column(2).Width = 20;
               



                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return new ExcelReportResponse(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");

                }
            }
        }

        private async Task<DataTable> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            var allPaymentType = await _context.PaymentTypes.ToListAsync(cancellationToken);

            DataTable dt = new DataTable();
            dt.TableName = "paymentType";
            dt.Columns.Add("Code", typeof(Guid));
            dt.Columns.Add("Code", typeof(string));

            var _list = _mapper.Map<List<PaymentType>>(allPaymentType);
            if (_list.Count > 0)
            {
                _list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.Name);

                });
            }

            return dt;
        }

    }


}
