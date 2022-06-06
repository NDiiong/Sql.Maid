﻿#pragma warning disable IDE1006

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Services.SqlControl;

namespace Sqlserver.maid.Commands.Grid
{
    public abstract class SqlGridResultCommand
    {
        private const string SQL_RESULT_GRID_CONTEXT_NAME = "SQL Results Grid Tab Context";
        protected static CommandBar SqlResultGridContext { get; }

        protected static readonly ISqlManagementService SqlManagementService;

        static SqlGridResultCommand()
        {
            SqlManagementService = new SqlManagementService();
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            SqlResultGridContext = ((CommandBars)dte.CommandBars)[SQL_RESULT_GRID_CONTEXT_NAME];
        }
    }
}