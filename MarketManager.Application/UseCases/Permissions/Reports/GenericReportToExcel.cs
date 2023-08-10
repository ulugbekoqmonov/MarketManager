using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketManager.Application.UseCases.Permissions.Reports;
public record GenericReportToExcel : IRequest<ExcelReportResponse>
{
    public string EndpoinName { get; set; }
}

public class GenericReportToExcelHandler : IRequestHandler<GenericReportToExcel, ExcelReportResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GenericReportToExcelHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExcelReportResponse> Handle(GenericReportToExcel request, CancellationToken cancellationToken)
    {
        var endpointName = request.EndpoinName;
        var dataTable = new DataTable();
        string str = "";
        try
        {
            str = WordSimilarityCalculator.CalculateSimilarity(endpointName.ToLower());
            dataTable = str switch
            {
                "user" => await GetEntitiesAsync<User>(await _context.Users.ToListAsync(cancellationToken), cancellationToken),
                "product" => await GetEntitiesAsync<Product>(await _context.Products.ToListAsync(cancellationToken), cancellationToken),
                "permission" => await GetEntitiesAsync<Permission>(await _context.Permissions.ToListAsync(cancellationToken), cancellationToken),
                "supplier" => await GetEntitiesAsync<Supplier>(await _context.Suppliers.ToListAsync(cancellationToken), cancellationToken),
                "client" => await GetEntitiesAsync<Client>(await _context.Clients.ToListAsync(cancellationToken), cancellationToken),
                "expiredProduct" => await GetEntitiesAsync<ExpiredProduct>(await _context.ExpiredProducts.ToListAsync(cancellationToken), cancellationToken),
                "role" => await GetEntitiesAsync<Role>(await _context.Roles.ToListAsync(cancellationToken), cancellationToken),
                "package" => await GetEntitiesAsync<Package>(await _context.Packages.ToListAsync(cancellationToken), cancellationToken),
                "paymentType" => await GetEntitiesAsync<PaymentType>(await _context.PaymentTypes.ToListAsync(cancellationToken), cancellationToken),
                "productType" => await GetEntitiesAsync<ProductType>(await _context.ProductTypes.ToListAsync(cancellationToken), cancellationToken),
                "order" => await GetEntitiesAsync<Order>(await _context.Orders.ToListAsync(cancellationToken), cancellationToken),
                "item" => await GetEntitiesAsync(await _context.Items.ToListAsync(cancellationToken), cancellationToken),
            };
        }
        catch (Exception ex)
        {

            throw new NotFoundException(nameof(endpointName), ex.Message);
        }

        using (XLWorkbook wb = new XLWorkbook())
        {
            var worksheet = wb.Worksheets.Add(dataTable);
            worksheet.Columns().AdjustToContents();
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return new ExcelReportResponse(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{str}.xlsx");
            }
        }
    }

    private Task<DataTable> GetEntitiesAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : BaseEntity
    {
        DataTable dt = new DataTable();
        dt.TableName = typeof(T).Name + "Data";
        foreach (var property in typeof(T).GetProperties())
        {
            if (property.PropertyType.AssemblyQualifiedName.Contains("System.Collections.Generic"))
            {
                continue;
            }
            else
            {
                dt.Columns.Add(property.Name, property.PropertyType);
            }

        }

        foreach (var entity in entities)
        {
            DataRow row = dt.NewRow();
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.PropertyType.AssemblyQualifiedName.Contains("System.Collections.Generic"))
                {
                    continue;
                }
                else
                {
                    row[property.Name] = property.GetValue(entity);

                }
            }
            dt.Rows.Add(row);
        }

        return Task.FromResult(dt);
    }
}


