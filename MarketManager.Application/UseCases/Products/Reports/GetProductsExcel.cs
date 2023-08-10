using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Products.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Products.Reports
{
    public class GetProductsExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetProductsExcelHandler : IRequestHandler<GetProductsExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsExcelHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetProductsExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook workbook = new())
            {
                var orderData = await GetProductsAsync(cancellationToken);
                var excelSheet = workbook.AddWorksheet(orderData, "Products");

                excelSheet.RowHeight = 20;
                excelSheet.Column(1).Width = 35;
                excelSheet.Column(2).Width = 15;
                excelSheet.Column(3).Width = 15;
                excelSheet.Column(4).Width = 35;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    return new ExcelReportResponse(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");
                }
            }
        }

        private async Task<DataTable> GetProductsAsync(CancellationToken cancellationToken = default)
        {
            var AllProducts = await _context.Products.ToListAsync(cancellationToken);

            DataTable excelDataTable = new()
            {
                TableName = "Empdata"
            };

            excelDataTable.Columns.Add("Id", typeof(Guid));
            excelDataTable.Columns.Add("Name", typeof(string));
            excelDataTable.Columns.Add("Description", typeof(string));
            excelDataTable.Columns.Add("ProductTypeId", typeof(Guid));

            var ProductsList = _mapper.Map<List<ProductResponse>>(AllProducts);

            if (ProductsList.Count > 0)
            {
                ProductsList.ForEach(item =>
                {
                    excelDataTable.Rows.Add(item.Id, item.Name, item.Description, item.ProductTypeId);
                });
            }

            return excelDataTable;
        }
    }
}
