using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus.Components
{
    internal sealed class SqlExportResultGridToExcelCommand : SqlCommandBase, ISqlCommand
    {
        private readonly SqlCommandBase _parentComponent;

        public SqlExportResultGridToExcelCommand(SqlCommandBase parentComponent) : base("Export to Excel")
        {
            _parentComponent = parentComponent;
        }

        public override Task ExcuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}