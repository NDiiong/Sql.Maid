using Microsoft.VisualStudio.Text.Editor;
using System;

namespace Sqlserver.maid.Services.SqlServiceProvider
{
    internal interface ISqlServiceProvider
    {
        IWpfTextView GetSqlWpfTextView(IServiceProvider serviceProvider);
    }
}