using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus
{
    public interface ISqlCommand : ISqlCommandBase
    {
        Task ExcuteAsync();
    }
}