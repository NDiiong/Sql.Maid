using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.Runtime;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands
{
    internal sealed class SqlNewIdAndCopyCommand
    {
        private static readonly IClipboardService _clipboardService;

        static SqlNewIdAndCopyCommand()
        {
            _clipboardService = new ClipboardService();
        }

        public static async Task InitializeAsync(Package package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var commandService = package.GetService<IMenuCommandService, OleMenuCommandService>();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            var cmdId = new CommandID(PackageGuids.guidCommands, PackageIds.PasteAsInsertedCommand);
            var menuItem = new OleMenuCommand((s, e) => Execute(dte), cmdId);
            menuItem.BeforeQueryStatus += (s, e) => CanExecute(s);
            commandService.AddCommand(menuItem);
        }

        private static void Execute(DTE2 dte)
        {
            var guid = Guid.NewGuid().ToString().ToUpper();
            _clipboardService.Set("\"" + guid + "\"");
        }

        private static void CanExecute(object s)
        {
            var oleMenuCommand = s as OleMenuCommand;
            oleMenuCommand.Visible = oleMenuCommand.Enabled = true;
        }
    }
}