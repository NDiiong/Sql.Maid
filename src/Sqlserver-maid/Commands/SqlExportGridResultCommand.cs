#pragma warning disable IDE1006

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Infrastructures.SqlControl;
using System;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands
{
    internal sealed class SqlExportGridResultCommand
    {
        private const string SQL_RESULT_GRID_CONTEXT_NAME = "SQL Results Grid Tab Context";

        private static readonly ISqlManagementService _sqlManagementService;

        static SqlExportGridResultCommand()
        {
            _sqlManagementService = new SqlManagementService();
        }

        public static async Task InitializeAsync(Package package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            var sqlResultGridContext = ((CommandBars)dte.CommandBars)[SQL_RESULT_GRID_CONTEXT_NAME];
            var btnInsertScriptControl = sqlResultGridContext.Controls
                .Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as CommandBarButton;

            btnInsertScriptControl.Visible = btnInsertScriptControl.Enabled = true;
            btnInsertScriptControl.Caption = "Generate Insert Script";
            btnInsertScriptControl.Click += (CommandBarButton _, ref bool __) => GenerateInsertScriptEventHandler(package, dte);
        }

        private static void GenerateInsertScriptEventHandler(IServiceProvider serviceProvider, DTE2 dte)
        {
            try
            {
                var currentGridControl = _sqlManagementService.GetCurrentGridControl(serviceProvider);
                if (currentGridControl != null)
                {
                    using (var gridResultControl = new GridResultControl(currentGridControl))
                    {
                        var columnHeaderList = gridResultControl.GetStringColumnHeadersInserted();
                        var contentRows = gridResultControl.GetStringRowsInserted();

                        var text = string.Join("\r\n,", contentRows.Select(line => $"({string.Join(", ", line)})"));
                        text = $"-- INSERT INTO #_tmp ({string.Join(", ", columnHeaderList)})\r\n"
                        + $"SELECT * FROM(VALUES \r\n {text}\r\n) AS _tab({string.Join(", ", columnHeaderList)})";

                        var textDoc = (TextDocument)dte.ActiveDocument.Object(null);
                        textDoc.EndPoint.CreateEditPoint().Insert(text);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}