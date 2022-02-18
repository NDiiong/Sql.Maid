using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus
{
    public interface ISqlCommandBase : ICommand
    {
        string Tooltip { get; set; }
        string Caption { get; }
    }
}