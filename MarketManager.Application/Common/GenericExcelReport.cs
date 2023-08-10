using AutoMapper;
using ClosedXML.Excel;
using MarketManager.Application.Common.Models;
using System.Data;

namespace MarketManager.Application.Common;
public class GenericExcelReport
{
    private readonly IMapper _mapper;

    public GenericExcelReport(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ExcelReportResponse> GetReportExcel<T, TMAP>(string filename, List<T> data, CancellationToken cancellationToken)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            var data2 = await GetDataAync<T, TMAP>(data);
            var sheet1 = wb.AddWorksheet(data2, nameof(T));

            sheet1.Columns().AdjustToContents(20.0,80.0);

            //sheet1.Column(1).Style.Font.FontColor = XLColor.Red;

            //sheet1.Columns(2, 4).Style.Font.FontColor = XLColor.Blue;

            //sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;

            //sheet1.Row(1).Style.Font.FontColor = XLColor.White;

            //sheet1.Row(1).Style.Font.Bold = true;
            //sheet1.Row(1).Style.Font.Shadow = true;
            //sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
            //sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
            //sheet1.Row(1).Style.Font.Italic = true;

            sheet1.RowHeight = 20;


            //sheet1.Column(1).Width = 38;
            //sheet1.Column(2).Width = 20;
            //sheet1.Column(3).Width = 20;
            //sheet1.Column(4).Width = 20;
            //sheet1.Column(5).Width = 20;
            //sheet1.Column(6).Width = 20;
            //sheet1.Column(7).Width = 20;
            //sheet1.Column(8).Width = 20;
            //sheet1.Column(9).Width = 20;



            using (MemoryStream ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                return new ExcelReportResponse(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{filename}.xlsx");

            }
        }
    }

    private async Task<DataTable> GetDataAync<T, TMAP>(List<T> data)
    {

        var properties = typeof(TMAP).GetProperties();


        DataTable dt = new DataTable();
        dt.TableName = typeof(T).Name;

        foreach (var property in properties)
        {
            string a = property.PropertyType.AssemblyQualifiedName;
            if (a.Contains("System.Collections.Generic"))
            {
                continue;
            }
            dt.Columns.Add(property.Name, property.PropertyType);
        }

        var _list = _mapper.Map<List<TMAP>>(data);
        if (_list.Count > 0)
        {
            foreach (var item in _list)
            {
                DataRow row = dt.NewRow();
                foreach (var prop in item.GetType().GetProperties())
                {
                    string a = prop.PropertyType.AssemblyQualifiedName;
                    if (a.Contains("System.Collections.Generic"))
                    {

                        continue;
                        //if (((IList)prop.GetValue(item)).Count > 0)
                        //{
                        //    foreach (var prop2 in (IList)prop.GetValue(item))
                        //    {
                        //        foreach (var prop3 in prop2.GetType().GetProperties())
                        //        {

                        //            if (prop3.Name != "Id")
                        //                row.SetField(prop3.Name, prop3.GetValue(prop2));

                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        row.SetField(prop.Name, prop.GetValue(item));

                    }

                }


                dt.Rows.Add(row);
            }
        }

        return await Task.FromResult(dt);
    }

}
