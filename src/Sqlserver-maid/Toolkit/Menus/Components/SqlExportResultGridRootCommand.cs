using Sqlserver.maid.Toolkit.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus.Components
{
    internal class SqlExportResultGridRootCommand : SqlCommandBase, ISqlCommand
    {
        private readonly ISqlResultGridContext _sqlResultGridContext;

        private List<SqlCommandBase> _components => new List<SqlCommandBase>();

        public SqlExportResultGridRootCommand(ISqlResultGridContext sqlResultGridContext) : base("Export")
        {
            _sqlResultGridContext = sqlResultGridContext;
        }

        public override Task ExcuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}