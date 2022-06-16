#pragma warning disable IDE1006

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.File;
using Sqlserver.maid.Services.SqlControl;
using System;
using System.IO;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands.Grid
{
    internal sealed class SqlSaveAsGridResultCommand : SqlGridResultCommand
    {
        public static async Task InitializeAsync(Package package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            var saveResultSpecialAsCommandBar = SqlResultGridContext.Controls
                .Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, Type.Missing, true)
                .Visible(true)
                .Caption("Save Result Special As...")
                .TooltipText("Sqlserver Maid - Tools for SQL Server Management Studio")
                .As<CommandBarPopup>();

            //Save Result As Json
            saveResultSpecialAsCommandBar.Controls
                .Add(MsoControlType.msoControlButton, 1, Type.Missing, Type.Missing, false)
                .Visible(true)
                .Caption("Save As Json")
                .TooltipText("Creates a JSON File based on the Grid Result.")
                .As<CommandBarButton>()
                .AddIcon("Assets/json.ico")
                .Click += (CommandBarButton _, ref bool __) => SqlSaveAsJsonGridResultEventHandler(package);

            //Save Result As Excel
            saveResultSpecialAsCommandBar.Controls
                .Add(MsoControlType.msoControlButton, 2, Type.Missing, Type.Missing, false)
                .Visible(true)
                .Caption("Save As Excel")
                .TooltipText("Creates a Excel File based on the Grid Result.")
                .As<CommandBarButton>()
                .AddIcon("Assets/ms.excel.ico")
                .Click += (CommandBarButton _, ref bool __) => SqlSaveAsExcelGridResultEventHandler(package);
        }

        private static void SqlSaveAsExcelGridResultEventHandler(IServiceProvider serviceProvider)
        {
            Function.Run(() =>
            {
                var saveDialog = new SaveFileDialog
                {
                    FileName = "",
                    Title = "Save Results As Excel",
                    Filter = "Excel (*.xlsx)|*.xlsx"
                };

                FileHandler(serviceProvider, saveDialog);
            });
        }

        private static void SqlSaveAsJsonGridResultEventHandler(IServiceProvider serviceProvider)
        {
            Function.Run(() =>
            {
                var saveDialog = new SaveFileDialog
                {
                    FileName = "",
                    Title = "Save Results As Json",
                    Filter = "Json (*.json)|*.json"
                };

                FileHandler(serviceProvider, saveDialog);
            });
        }

        private static void FileHandler(IServiceProvider serviceProvider, SaveFileDialog saveDialog)
        {
            var diaglogResult = saveDialog.ShowDialog();
            if (diaglogResult != DialogResult.Cancel)
            {
                var extension = Path.GetExtension(saveDialog.FileName).ToLower();
                var fileservice = FileServiceBase.GetService(extension);

                if (fileservice != null)
                {
                    var currentGridControl = SqlManagementService.GetCurrentGridControl(serviceProvider);
                    if (currentGridControl != null)
                    {
                        using (var gridResultControl = new GridResultControl(currentGridControl))
                        {
                            fileservice.WriteFile(saveDialog.FileName, gridResultControl.AsDatatable());
                        }
                    }
                }
            }
        }
    }
}