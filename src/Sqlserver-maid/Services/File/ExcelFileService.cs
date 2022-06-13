using ClosedXML.Excel;
using System.Data;

namespace Sqlserver.maid.Services.File
{
    public class ExcelFileService : IFileService
    {
        public string AsJson(DataTable datatable)
        {
            return string.Empty;
        }

        public void WriteFile(string path, DataTable datatable)
        {
            using (var workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var workSheet = workbook.Worksheets.Add(datatable, "GridResult");
                workSheet.Columns().AdjustToContents();
                workbook.SaveAs(path);
            }
        }
    }
}