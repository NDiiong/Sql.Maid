using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;

namespace Sqlserver.maid.Extension
{
    public static class ServiceProviderExtension
    {
        public static IWpfTextView GetWpfTextView(this IServiceProvider serviceProvider)
        {
            var textManager = (IVsTextManager)serviceProvider.GetService(typeof(SVsTextManager));
            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            var editor = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            textManager.GetActiveView(1, null, out IVsTextView textViewCurrent);
            return editor.GetWpfTextView(textViewCurrent);
        }
    }
}