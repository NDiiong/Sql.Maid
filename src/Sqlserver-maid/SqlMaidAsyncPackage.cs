using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Commands;
using Sqlserver.maid.Options;
using Sqlserver.maid.Services;
using System;
using System.Runtime.InteropServices;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid
{
    [Guid(PackageGuids.guidSqlMaidPackageString)]
    [InstalledProductRegistration("#110", "#112", Vsix.Version, IconResourceID = 400)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "sqlserver maid", "General", 100, 101, true)]
    public sealed class SqlMaidAsyncPackage : SqlAsyncPackage
    {
        public SqlMaidAsyncPackage() : base(PackageGuids.guidSqlMaidPackageString)
        {
        }

        protected override async Task InitializeAsync()
        {
            await SqlJoinLinesCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlPasteAsCsvCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlQueryTemplateCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlExportGridResultCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlPasteAsInsertedQueryCommand.InitializeAsync(this).ConfigureAwait(false);
            await SqlPasteAsInsertedHeaderQueryCommand.InitializeAsync(this).ConfigureAwait(false);
        }
    }
}