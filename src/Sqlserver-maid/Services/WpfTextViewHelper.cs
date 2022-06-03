using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Sqlserver.maid.Services
{
    internal static class WpfTextViewHelper
    {
        public static int GetCaretPosition(IWpfTextView wpfTextView)
        {
            return wpfTextView.Caret.Position.BufferPosition.Position;
        }

        internal static IWpfTextView GetTextView()
        {
            var compService = ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel)) as IComponentModel;

            var editorAdapter = compService.GetService<IVsEditorAdaptersFactoryService>();
            return editorAdapter.GetWpfTextView(GetCurrentNativeTextView());
        }

        internal static IVsTextView GetCurrentNativeTextView()
        {
            var textManager = ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;

            ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out var activeView));
            return activeView;
        }
    }
}