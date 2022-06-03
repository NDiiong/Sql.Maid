using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.Grid;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Sqlserver.maid.Infrastructures;
using Sqlserver.maid.Infrastructures.Control;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands
{
    internal sealed class SqlExportGridResultCommand
    {
        public static async Task InitializeAsync(Package package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            var tabContext = ((CommandBars)dte.CommandBars)["SQL Results Grid Tab Context"];
            var btnSaveToScript = tabContext.Controls
                .Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as CommandBarButton;
            btnSaveToScript.Caption = "SpaceX: Save data to script";
            btnSaveToScript.Click += (CommandBarButton _, ref bool __) => BtnSaveToScript_Click(package, dte);
        }

        private static void BtnSaveToScript_Click(IServiceProvider serviceProvider, DTE2 dte)
        {
            var gridControl = GetCurrentGridControl(serviceProvider);
            if (gridControl != null)
            {
                var controller = new GridResultControl(gridControl);

                GetGridData(gridControl, out List<string> columnHeaderList, out List<List<string>> data);

                string result = string.Join("\r\n,", data.Select(line => $"({string.Join(", ", line)})"));

                result = $"-- INSERT INTO #tmp_GridResults ({string.Join(", ", columnHeaderList)})\r\n"
                    + $"select * from(values \r\n {result}\r\n) as T({string.Join(", ", columnHeaderList)})";

                var textDoc = (TextDocument)dte.ActiveDocument.Object(null);
                textDoc.EndPoint.CreateEditPoint().Insert(result);
            }
        }

        private static void GetGridData(GridControl gridControl, out List<string> columnHeaderList, out List<List<string>> data)
        {
            int columnsNumber = gridControl.ColumnsNumber;
            long totalRows = gridControl.GridStorage.NumRows();
            columnHeaderList = new List<string>(columnsNumber);
            for (int j = 1; j < columnsNumber; j++)
            {
                string text;
                gridControl.GetHeaderInfo(j, out text, out Bitmap bitmap);
                if (columnHeaderList.Contains("[" + text + "]"))
                {
                    text = text + "_" + j.ToString();
                }
                columnHeaderList.Add("[" + text + "]");
            }

            data = new List<List<string>>();

            for (long rowNum = 0L; rowNum < totalRows; rowNum += 1L)
            {
                var row = new List<string>();
                for (int colNum = 1; colNum < columnsNumber; colNum++)
                {
                    string cellText = gridControl.GridStorage.GetCellDataAsString(rowNum, colNum) ?? "";
                    cellText = cellText.Replace("'", "''");
                    if (true)
                    {
                        cellText = cellText.Trim();
                    }
                    if (cellText.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                    {
                    }
                    else
                    {
                        cellText = "N'" + cellText + "'";
                    }
                    row.Add(cellText);
                }

                data.Add(row);
            }
        }

        private static GridControl GetCurrentGridControl(IServiceProvider serviceProvider)
        {
            var ms = serviceProvider.GetService(typeof(IVsMonitorSelection)) as IVsMonitorSelection;

            ms.GetCurrentElementValue((int)VSConstants.VSSELELEMID.SEID_WindowFrame, out object _vsWindowFrame);
            var vsWindowFrame = _vsWindowFrame as IVsWindowFrame;

            vsWindowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out var _control);
            switch (_control)
            {
                case Control control:
                    return Function.Run(() => { return (GridControl)((ContainerControl)((ContainerControl)control).ActiveControl).ActiveControl; });
            }

            return null;
        }
    }
}