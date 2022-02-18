using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Toolkit
{
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string)]
    public abstract class SqlAsyncPackage : Package
    {
        private readonly string _packageGuidString;

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

            ThreadHelper.JoinableTaskFactory.Run(InitializeAsync);
        }

        private void SetSkipLoading()
        {
            try
            {
                RegistryKey registryKey = UserRegistryRoot.CreateSubKey(
                    string.Format("Packages\\{{{0}}}", _packageGuidString));

                registryKey.SetValue("SkipLoading", 1, RegistryValueKind.DWord);
                registryKey.Close();
            }
            catch
            { }
        }
    }
}