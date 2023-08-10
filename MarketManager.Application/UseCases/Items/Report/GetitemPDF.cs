using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MarketManager.Application.Common.Interfaces;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Import.Export;

public record GetItemPDF(string FileName) : IRequest<PDFExportResponse>;

public class GetItemPDFHandler : IRequestHandler<GetItemPDF, PDFExportResponse>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetItemPDFHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }

    public async Task<PDFExportResponse> Handle(GetItemPDF request, CancellationToken cancellationToken)
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

            PdfPCell headerCell = new PdfPCell(new Phrase("Items", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            headerCell.Colspan = 5;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(headerCell);
            table.CompleteRow();


            table.AddCell("ID");
            table.AddCell("ProductId");
            table.AddCell("OrderId");
            table.AddCell("Count");
            table.AddCell("Sold Price");
            table.CompleteRow();

            foreach (var item in _context.Items)
            {
                table.AddCell(item.Id.ToString());
                table.AddCell($"{item.ProductId}");
                table.AddCell(item.OrderId.ToString());
                table.AddCell(item.Amount.ToString());
                table.AddCell(item.TotalPrice.ToString());
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
