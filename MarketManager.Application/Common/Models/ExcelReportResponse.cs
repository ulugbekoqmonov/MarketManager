namespace MarketManager.Application.Common.Models;
public record ExcelReportResponse(byte[] FileContents, string Option, string FileName);