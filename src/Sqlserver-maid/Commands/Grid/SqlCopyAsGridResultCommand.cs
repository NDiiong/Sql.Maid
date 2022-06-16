#pragma warning disable IDE1006

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.File;
using Sqlserver.maid.Services.Runtime;
using Sqlserver.maid.Services.SqlControl;
using System;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands.Grid
{
    internal sealed class SqlCopyAsGridResultCommand : SqlGridResultCommand
    {
        private static readonly IClipboardService _clipboardService;

        static SqlCopyAsGridResultCommand()
        {
            _clipboardService = new ClipboardService();
        }

        public static async Task InitializeAsync(Package package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            var copyAsCommandBar = SqlResultGridContext.Controls
                .Add(MsoControlType.msoControlPopup, Type.Missing, Type.Missing, Type.Missing, true)
                .Visible(true)
                .Caption("Copy As...")
                .TooltipText("Sqlserver Maid - Tools for SQL Server Management Studio")
                .As<CommandBarPopup>();

            //Save Result As Json
            copyAsCommandBar.Controls
                .Add(MsoControlType.msoControlButton, 1, Type.Missing, Type.Missing, false)
                .Visible(true)
                .Caption("Copy As Json")
                .TooltipText("Copy As Json based on the Grid Result.")
                .As<CommandBarButton>()
                //.AddIcon(VSPackage.json_file)
                .Click += (CommandBarButton _, ref bool __) => SqlCopyAsJsonGridResultEventHandler(package);
        }

        private static void SqlCopyAsJsonGridResultEventHandler(IServiceProvider serviceProvider)
        {
            Function.Run(() =>
            {
                var currentGridControl = SqlManagementService.GetCurrentGridControl(serviceProvider);
                if (currentGridControl != null)
                {
                    using (var gridResultControl = new GridResultControl(currentGridControl))
                    {
                        var json = FileServiceBase.JsonService.AsJson(gridResultControl.AsDatatable());

                        if (!string.IsNullOrEmpty(json))
                            _clipboardService.Set(json);
                    }
                }
            });
        }
    }
}