using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using Sqlserver.maid.Services.Extension;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Services.SqlPackage
{
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string)]
    public abstract class SqlAsyncPackage : Package
    {
        private readonly string _packageGuidString;

        public DTE2 Application { get; private set; }
        public IMenuCommandService MenuCommand { get; private set; }

        public SqlAsyncPackage(string packageGuidString)
        {
            _packageGuidString = packageGuidString;
        }

        protected override int QueryClose(out bool canClose)
        {
#if !DEBUG
            SetSkipLoading();
#endif
            return base.QueryClose(out canClose);
        }

        protected abstract Task InitializeAsync();

        protected override void Initialize()
        {
            base.Initialize();
            Application = GetGlobalService(typeof(DTE)) as DTE2;
            MenuCommand = GetService(typeof(IMenuCommandService)).As<IMenuCommandService>();
            ThreadHelper.JoinableTaskFactory.Run(InitializeAsync);
        }

        private void SetSkipLoading()
        {
            try
            {
                var registryKey = UserRegistryRoot.CreateSubKey(string.Format("Packages\\{{{0}}}", _packageGuidString));
                registryKey.SetValue("SkipLoading", 1, RegistryValueKind.DWord);
                registryKey.Close();
            }
            catch
            { }
        }
    }
}