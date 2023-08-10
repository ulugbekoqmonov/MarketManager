using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MarketManager.Application.Common.Interfaces;
using MediatR;

namespace MarketManager.Application.UseCases.Orders.Import.Export;

public record GetOrderPDF(string FileName) : IRequest<PDFExportResponse>;

public class GetOrderPDFHandler : IRequestHandler<GetOrderPDF, PDFExportResponse>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderPDFHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }

    public async Task<PDFExportResponse> Handle(GetOrderPDF request, CancellationToken cancellationToken)
    {

        using (MemoryStream ms = new MemoryStream())
        {
            Document document = new Document();
            document.SetMargins(20, 20, 40, 40);
            document.SetPageSize(PageSize.A4);

            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            HeaderFooterHelper headerFooter = new HeaderFooterHelper();
            writer.PageEvent = headerFooter;

            document.Open();

            PdfPTable table = new PdfPTable(5);

            table.SetWidths(new float[] { 2f, 0.5f, 0.5f, 0.5f, 1.5f });

            table.SpacingBefore = 10;
            table.SpacingAfter = 10;

            PdfPCell headerCell = new PdfPCell(new Phrase("Orders", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            headerCell.Colspan = 5;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(headerCell);
            table.CompleteRow();


            table.AddCell("ID");
            table.AddCell("Total Price");
            table.AddCell("Card Price Sum");
            table.AddCell("CashPurchaseSum");
            table.AddCell("Client ID");
            table.CompleteRow();

            foreach (var order in _context.Orders)
            {
                table.AddCell(order.Id.ToString());
                table.AddCell($"{order.TotalPrice}");
                table.AddCell(order.CashbackSum.ToString());
                table.AddCell(order.TotalPriceBeforeCashback.ToString());
                table.AddCell(order.ClientId.ToString());
                table.CompleteRow();
            }

            document.Add(table);
            document.Close();

            return await Task.FromResult(new PDFExportResponse(ms.ToArray(), "application/pdf", request.FileName));
        }
    }
}
public class HeaderFooterHelper : PdfPageEventHelper
{
    public override void OnEndPage(PdfWriter writer, Document document)
    {
        PdfPTable footerTable = new PdfPTable(1);
        footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        footerTable.DefaultCell.Border = Rectangle.NO_BORDER;
        footerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        footerTable.AddCell(new Phrase($"Date: {DateTime.Now.ToString("yyyy-MM-dd")}", new Font(Font.FontFamily.HELVETICA, 8)));

        footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
    }
}

public record PDFExportResponse(byte[] FileContents, string Options, string FileName);
