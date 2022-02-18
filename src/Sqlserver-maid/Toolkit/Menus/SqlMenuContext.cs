using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Sqlserver.maid.Toolkit.Extension;

namespace Sqlserver.maid.Toolkit.Menus
{
    public abstract class SqlMenuContext : ISqlMenuContext
    {
        protected abstract string ContextName { get; }

        private static CommandBar _context;
        public CommandBar Context => _context ?? (_context = GetResultGridContext());

        private CommandBar GetResultGridContext()
        {
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            var commandBars = dte.CommandBars;

            return commandBars.As<CommandBars>()[ContextName];
        }
    }
}