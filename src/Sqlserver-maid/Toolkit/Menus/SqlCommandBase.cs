using System.Threading.Tasks;

namespace Sqlserver.maid.Toolkit.Menus
{
    internal abstract class SqlCommandBase : ISqlCommandBase
    {
        public bool Enabled { get; set; } = true;
        public bool Visible { get; set; } = true;
        public string Tooltip { get; set; }
        public string Caption { get; }

        protected SqlCommandBase(string caption)
        {
            Caption = caption;
        }

        public abstract Task ExcuteAsync();
    }
}