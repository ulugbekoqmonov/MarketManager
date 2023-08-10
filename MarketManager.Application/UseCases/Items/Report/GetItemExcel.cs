using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Items.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Items.Import.Export
{
    public class GetItemExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetItemExcelHandler : IRequestHandler<GetItemExcel, ExcelReportResponse>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetItemExcelHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetItemExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook wb = new())
            {
                var itemData = await GetItemAsync(cancellationToken);
                var sheet1 = wb.AddWorksheet(itemData, "Items");


                sheet1.Column(1).Style.Font.FontColor = XLColor.Red;

                sheet1.Columns(2, 4).Style.Font.FontColor = XLColor.Blue;

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
                sheet1.Column(3).Width = 20;
                sheet1.Column(4).Width = 20;
                sheet1.Column(5).Width = 38;



                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return new ExcelReportResponse(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");

                }
            }
        }

        private async Task<DataTable> GetItemAsync(CancellationToken cancellationToken = default)
        {
            var AllItems = await _context.Items.ToListAsync(cancellationToken);

            DataTable dt = new()
            {
                TableName = "Empdata"
            };
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("ProductId", typeof(Guid));
            dt.Columns.Add("OrderId", typeof(Guid));
            dt.Columns.Add("Amount", typeof(double));
            dt.Columns.Add("TotalPrice", typeof(double));


            var _list = _mapper.Map<List<ItemResponse>>(AllItems);
            if (_list.Count > 0)
            {
                _list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.ProductId, item.OrderId, item.Amount, item.TotalPrice);

                });
            }

            return dt;
        }

    }
}

