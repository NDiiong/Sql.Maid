using Microsoft.VisualStudio.CommandBars;

namespace Sqlserver.maid.Toolkit.Menus
{
    public interface ISqlMenuContext
    {
        CommandBar Context { get; }
    }
}