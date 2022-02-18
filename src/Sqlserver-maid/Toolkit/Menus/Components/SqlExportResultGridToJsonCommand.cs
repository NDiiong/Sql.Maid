using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus.Components
{
    internal sealed class SqlExportResultGridToJsonCommand : SqlCommandBase, ISqlCommand
    {
        private readonly SqlCommandBase _parentComponent;

        public SqlExportResultGridToJsonCommand(SqlCommandBase parentComponent) : base("Export to Json")
        {
            _parentComponent = parentComponent;
        }

        public override Task ExcuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}