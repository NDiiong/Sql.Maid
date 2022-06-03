using Microsoft.SqlServer.Management.UI.Grid;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Windows.Forms;

namespace Sqlserver.maid.Infrastructures.SqlControl
{
    public class SqlManagementService : ISqlManagementService
    {
        public GridControl GetCurrentGridControl(IServiceProvider serviceProvider)
        {
            var ms = serviceProvider.GetService(typeof(IVsMonitorSelection)) as IVsMonitorSelection;

            ms.GetCurrentElementValue((int)VSConstants.VSSELELEMID.SEID_WindowFrame, out object _vsWindowFrame);
            var vsWindowFrame = _vsWindowFrame as IVsWindowFrame;

            vsWindowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out var _control);
            switch (_control)
            {
                case Control control:
                    return (GridControl)((ContainerControl)((ContainerControl)control).ActiveControl).ActiveControl;
            }

            return null;
        }
    }
}