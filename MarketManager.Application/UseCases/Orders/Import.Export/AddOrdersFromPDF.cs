using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Orders.Response;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Spire.Pdf;
using Spire.Pdf.Utilities;

namespace MarketManager.Application.UseCases.Orders.Import.Export;

public record AddOrdersFromPDF(IFormFile PdfFile) : IRequest<List<OrderResponse>>;

public class AddOrdersFromPDFHandler : IRequestHandler<AddOrdersFromPDF, List<OrderResponse>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddOrdersFromPDFHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderResponse>> Handle(AddOrdersFromPDF request, CancellationToken cancellationToken)
    {
        byte[] PdfInBytes = ConvertFormFileToByteArray(request.PdfFile);
        PdfDocument pdf = new(PdfInBytes);
        List<OrderResponse> result = new();
        for (int pageIndex = 0; pageIndex < pdf.Pages.Count; pageIndex++)
        {
            PdfTableExtractor extractor = new(pdf);
            PdfTable[] tableLists = extractor.ExtractTable(pageIndex);
            if (tableLists != null && tableLists.Length > 0)
            {
                foreach (PdfTable table in tableLists)
                {
                    for (int i = 2; i < table.GetRowCount(); i++)
                    {
                        Order order = new()
                        {
                            //TotalPrice = double.Parse(table.GetText(i, 1)),
                            CashbackSum = double.Parse(table.GetText(i, 2)),
                            TotalPriceBeforeCashback = double.Parse(table.GetText(i, 3)),
                            ClientId = Guid.Parse(table.GetText(i, 4).Replace("\n", ""))
                        };

                        await _context.Orders.AddAsync(order);
                        result.Add(_mapper.Map<OrderResponse>(order));
                    }
                }
            }
        }
        await _context.SaveChangesAsync();
        return result;
    }
    public byte[] ConvertFormFileToByteArray(IFormFile file)
    {
        using MemoryStream ms = new();
        file.CopyTo(ms);
        return ms.ToArray();
    }
}
