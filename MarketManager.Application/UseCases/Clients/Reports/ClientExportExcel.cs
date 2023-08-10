using MarketManager.Application.Common;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace MarketManager.Application.UseCases.Clients.Reports;

public class ClientExportExcel : IRequest<ExcelReportResponse>
{
    public string FileName { get; set; }
}
public class TestGetExcelClientHandler : IRequestHandler<ClientExportExcel, ExcelReportResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly GenericExcelReport _generic;

    public TestGetExcelClientHandler(IApplicationDbContext context, GenericExcelReport generic)
    {
        _context = context;
        _generic = generic;

    }

    public async Task<ExcelReportResponse> Handle(ClientExportExcel request, CancellationToken cancellationToken)
    {

        var result = await _generic.GetReportExcel<Client, ClientsResponseExcelReport>(request.FileName, await _context.Clients.ToListAsync(cancellationToken), cancellationToken);
        return result;
    }
}

public class ClientsResponseExcelReport
{
    public Guid Id { get; set; }
    public string? PhoneNumber { get; set; }
    public double CashbackSum { get; set; }
}
