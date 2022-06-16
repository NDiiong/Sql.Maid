using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services.Extension;
using Sqlserver.maid.Services.Loging;
using Sqlserver.maid.Services.SqlPackage;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands.Excute
{
    internal sealed class QueryHistoryCommand
    {
        public static async Task InitializeAsync(SqlAsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var commandService = package.GetService<IMenuCommandService, OleMenuCommandService>();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;

            dte.Events.CommandEvents.AfterExecute += CommandEvents_AfterExecute;
        }

        private static void CommandEvents_AfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            Logger.Info("Invoke: CommandEvents_AfterExecute");
        }
    }
}