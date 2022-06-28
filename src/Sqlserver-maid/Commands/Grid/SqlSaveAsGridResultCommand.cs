﻿#pragma warning disable IDE1006

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.File;
using Sqlserver.maid.Services.SqlControl;
using Sqlserver.maid.Services.SqlPackage;
using System;
using System.IO;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands.Grid
{
    internal sealed class SqlSaveAsGridResultCommand : SqlGridResultCommand
    {
        public static async Task InitializeAsync(SqlAsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            var localPath = package.GetExtensionInstallationDirectory();
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
                .AddIcon($"{localPath}/Assets/json.ico")
                .Click += (CommandBarButton _, ref bool __) => SqlSaveAsJsonGridResultEventHandler(package, dte);

            //Save Result As Excel
            saveResultSpecialAsCommandBar.Controls
                .Add(MsoControlType.msoControlButton, 2, Type.Missing, Type.Missing, false)
                .Visible(true)
                .Caption("Save As Excel")
                .TooltipText("Creates a Excel File based on the Grid Result.")
                .As<CommandBarButton>()
                .AddIcon($"{localPath}/Assets/ms.excel.ico")
                .Click += (CommandBarButton _, ref bool __) => SqlSaveAsExcelGridResultEventHandler(package, dte);
        }

        private static void SqlSaveAsExcelGridResultEventHandler(IServiceProvider serviceProvider, DTE2 dte)
        {
            Function.Run(() =>
            {
                var saveDialog = new SaveFileDialog
                {
                    FileName = Path.GetTempFileName(),
                    Title = "Save Results As Excel",
                    Filter = "Excel (*.xlsx)|*.xlsx",
                    DefaultExt = "xlsx",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                };

                FileHandler(serviceProvider, saveDialog, dte);
            });
        }

        private static void SqlSaveAsJsonGridResultEventHandler(IServiceProvider serviceProvider, DTE2 dte)
        {
            Function.Run(() =>
            {
                var saveDialog = new SaveFileDialog
                {
                    FileName = Path.GetTempFileName(),
                    Title = "Save Results As Json",
                    Filter = "Json (*.json)|*.json",
                    DefaultExt = "json",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                };

                FileHandler(serviceProvider, saveDialog, dte);
            });
        }

        private static void FileHandler(IServiceProvider serviceProvider, SaveFileDialog saveDialog, DTE2 dte)
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
                            dte.StatusBar.Text = "Successed";
                        }
                    }
                }
            }
        }
    }
}