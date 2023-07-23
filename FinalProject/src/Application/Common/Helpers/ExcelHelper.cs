
using Application.Features.Products.Queries.GetAllByOrderId;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public class ExcelHelper
    {
        
        public Byte[] DtoToExcelConvertion(List<ProductGetAllDto> productsGetAllDto)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("Sheet1");

            IRow rowHeader= sheet1.CreateRow(0);
            rowHeader.HeightInPoints = 30;
    

           ICellStyle headerStyle = workbook.CreateCellStyle();
         
            IFont headerFont = workbook.CreateFont();
            headerFont.IsBold= true;
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.SetFont(headerFont);

        
            rowHeader.CreateCell(0).SetCellValue("Id");
            rowHeader.GetCell(0).CellStyle = headerStyle;
            rowHeader.CreateCell(1).SetCellValue("OrderId");
            rowHeader.GetCell(1).CellStyle = headerStyle;
            rowHeader.CreateCell(2).SetCellValue("Name");
            rowHeader.GetCell(2).CellStyle = headerStyle;
            rowHeader.CreateCell(3).SetCellValue("Picture");
            rowHeader.GetCell(3).CellStyle = headerStyle;
            rowHeader.CreateCell(4).SetCellValue("IsOnSale");
            rowHeader.GetCell(4).CellStyle = headerStyle;
            rowHeader.CreateCell(5).SetCellValue("Price");
            rowHeader.GetCell(5).CellStyle = headerStyle;
            rowHeader.CreateCell(6).SetCellValue("SalePrice");
            rowHeader.GetCell(6).CellStyle = headerStyle;

            int index = 1;
            foreach(ProductGetAllDto product in productsGetAllDto)
            {
                IRow row = sheet1.CreateRow(index);
                row.CreateCell(0).SetCellValue(product.Id.ToString());
                row.CreateCell(1).SetCellValue(product.OrderId.ToString());
                row.CreateCell(2).SetCellValue(product.Name.ToString());
                row.CreateCell(3).SetCellValue(product.Picture.ToString());
                row.CreateCell(4).SetCellValue(product.IsOnSale.ToString());
                row.CreateCell(5).SetCellValue(double.Parse(product.Price.ToString()));
                row.CreateCell(6).SetCellValue(double.Parse(product.SalePrice.ToString()));

                // Set the height of the row to a fixed value (20 points)
                row.HeightInPoints = 25;

                // Apply vertical alignment to content rows
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.VerticalAlignment = VerticalAlignment.Center;
                contentStyle.Alignment = HorizontalAlignment.Center;
             
                for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                {
                    row.GetCell(cellIndex).CellStyle = contentStyle;
                }

                index++;
            }

         
             int columnCount = rowHeader.LastCellNum;
             int desiredWidth = 40; 

             for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
             {
                 sheet1.SetColumnWidth(columnIndex, desiredWidth * 256);
             }


            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData, false);
               

                byte[] bytes = exportData.ToArray();
                return bytes;
            }
        }


    }
}
