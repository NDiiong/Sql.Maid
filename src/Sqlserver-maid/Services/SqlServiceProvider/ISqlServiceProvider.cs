using Microsoft.VisualStudio.Text.Editor;
using System;

namespace Sqlserver.maid.Services.SqlTextView
{
    internal interface ISqlServiceProvider
    {
        IWpfTextView GetSqlWpfTextView(IServiceProvider serviceProvider);
    }
}