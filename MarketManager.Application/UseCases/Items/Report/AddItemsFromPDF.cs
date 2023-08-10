using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Items.Queries.GetAllItems;
using MarketManager.Application.UseCases.Items.Response;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Spire.Pdf;
using Spire.Pdf.Utilities;

namespace MarketManager.Application.UseCases.Items.Import.Export
{
    public record AddItemsFromPDF(IFormFile PdfFile) : IRequest<List<ItemResponse>>;

    public class AddItemsFromPDFHandler : IRequestHandler<AddItemsFromPDF, List<ItemResponse>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddItemsFromPDFHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ItemResponse>> Handle(AddItemsFromPDF request, CancellationToken cancellationToken)
        {
            byte[] PdfInBytes = ConvertFormFileToByteArray(request.PdfFile);
            PdfDocument pdf = new(PdfInBytes);
            List<ItemResponse> result = new();
            for (int pageIndex = 0; pageIndex < pdf.Pages.Count; pageIndex++)
            {
                PdfTableExtractor extractor = new PdfTableExtractor(pdf);
                PdfTable[] tableLists = extractor.ExtractTable(pageIndex);
                if (tableLists != null && tableLists.Length > 0)
                {
                    foreach (PdfTable table in tableLists)
                    {
                        for (int i = 2; i < table.GetRowCount(); i++)
                        {
                            Item item = new()
                            {
                                ProductId = Guid.Parse(table.GetText(i, 1)),
                                OrderId = Guid.Parse(table.GetText(i, 2)),
                                Amount = double.Parse(table.GetText(i, 3)),
                                TotalPrice = double.Parse(table.GetText(i, 4).Replace("\n", ""))
                            };

                            await _context.Items.AddAsync(item);
                            result.Add(_mapper.Map<ItemResponse>(item));
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            return result;
        }
        public byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

}
