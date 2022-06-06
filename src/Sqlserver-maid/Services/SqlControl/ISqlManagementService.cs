using Microsoft.SqlServer.Management.UI.Grid;
using System;

namespace Sqlserver.maid.Services.SqlControl
{
    public interface ISqlManagementService
    {
        GridControl GetCurrentGridControl(IServiceProvider serviceProvider);
    }
}