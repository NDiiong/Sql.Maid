using Microsoft.SqlServer.Management.UI.Grid;
using System;

namespace Sqlserver.maid.Infrastructures.SqlControl
{
    public interface ISqlManagementService
    {
        GridControl GetCurrentGridControl(IServiceProvider serviceProvider);
    }
}