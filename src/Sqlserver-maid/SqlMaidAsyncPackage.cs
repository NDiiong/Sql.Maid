using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Commands;
using Sqlserver.maid.Commands.Excute;
using Sqlserver.maid.Commands.Grid;
using Sqlserver.maid.Services.SqlPackage;
using System;
using System.Runtime.InteropServices;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid
{
    [Guid(PackageGuids.guidSqlMaidPackageString)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    public sealed class SqlMaidAsyncPackage : SqlAsyncPackage
    {
        public SqlMaidAsyncPackage() : base(PackageGuids.guidSqlMaidPackageString)
        {
        }

        protected override async Task InitializeAsync()
        {
            //await WindowEventLogging.InitializeAsync(this).ConfigureAwait(false);
            await QueryHistoryCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlJoinLinesCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlPasteAsCsvCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlCopyAsGridResultCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlSaveAsGridResultCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlPasteAsInsertedQueryCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlPasteAsInsertedHeaderQueryCommand.InitializeAsync(this).ConfigureAwait(false);
            //await SqlInsertScriptGridResultCommand.InitializeAsync(this).ConfigureAwait(false);
        }
    }
}