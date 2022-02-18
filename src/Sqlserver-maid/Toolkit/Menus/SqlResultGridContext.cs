using Sqlserver.maid.Toolkit.Services;

namespace Sqlserver.maid.Toolkit.Menus
{
    public class SqlResultGridContext : SqlMenuContext, ISqlResultGridContext
    {
        private const string _grdResultContextName = "SQL Results Grid Tab Context";
        protected override string ContextName => _grdResultContextName;
    }
}