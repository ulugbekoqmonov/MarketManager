using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Products.Response;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.Products.Reports
{

    public record AddProductsFromExcel(IFormFile ExcelFile) : IRequest<List<ProductResponse>>;

    public class AddProductsFromExcelHandler : IRequestHandler<AddProductsFromExcel, List<ProductResponse>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddProductsFromExcelHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductResponse>> Handle(AddProductsFromExcel request, CancellationToken cancellationToken)
        {
            if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                throw new ArgumentNullException("File", "file is empty or null");

            var file = request.ExcelFile;
            List<Product> result = new();
            using (var ms = new MemoryStream())
            {

                await file.CopyToAsync(ms, cancellationToken);
                using (var wb = new XLWorkbook(ms))
                {
                    var sheet1 = wb.Worksheet(1);
                    int startRow = 2;
                    for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                    {
                        var product = new Product()
                        {
                            Name = sheet1.Cell(row, 1).GetString(),
                            Description = sheet1.Cell(row, 2).GetString(),
                            ProductTypeId = Guid.Parse(sheet1.Cell(row, 3).GetString())
                        };

                        result.Add(product);
                    }
                }
            }
            await _context.Products.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return _mapper.Map<List<ProductResponse>>(result);
        }
    }
}
