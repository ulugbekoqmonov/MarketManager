using ClosedXML.Excel;
using MarketManager.Application.Common;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketManager.Application.UseCases.Suppliers.Report;

public class SupplierExportExcel : IRequest<ExcelReportResponse>
{
    public string FileName { get; set; }
}
public class TestGetExcelSupplierHandler : IRequestHandler<SupplierExportExcel, ExcelReportResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly GenericExcelReport _generic;

    public TestGetExcelSupplierHandler(IApplicationDbContext context, GenericExcelReport generic)
    {
        _context = context;
        _generic = generic;

    }

    public async Task<ExcelReportResponse> Handle(SupplierExportExcel request, CancellationToken cancellationToken)
    {

        var result = await _generic.GetReportExcel<Supplier, SuppliersResponseExcelReport>(request.FileName, await _context.Suppliers.ToListAsync(cancellationToken), cancellationToken);
        return result;
    }
}

public class SuppliersResponseExcelReport
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
}