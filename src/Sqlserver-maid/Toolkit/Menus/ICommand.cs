using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus
{
    public interface ICommand
    {
        bool Enabled { get; set; }
        bool Visible { get; set; }
    }
}