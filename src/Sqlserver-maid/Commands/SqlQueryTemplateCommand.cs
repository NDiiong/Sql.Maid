using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Sqlserver.maid.Services;
using System;
using Constants = Microsoft.VisualStudio.OLE.Interop.Constants;
using Task = System.Threading.Tasks.Task;

namespace Sqlserver.maid.Commands
{
    public class SqlQueryTemplateCommand : IOleCommandTarget
    {
        private readonly DTE2 _dte;
        private readonly SqlAsyncPackage _sqlAsyncPackage;
        private readonly ITextDocumentService _textDocumentService;

        public SqlQueryTemplateCommand(SqlAsyncPackage sqlAsyncPackage, DTE2 dte, ITextDocumentService textDocumentService)
        {
            _dte = dte;
            _sqlAsyncPackage = sqlAsyncPackage;
            _textDocumentService = textDocumentService;
        }

        internal static async Task InitializeAsync(SqlAsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            var pct = Package.GetGlobalService(typeof(SVsRegisterPriorityCommandTarget)) as IVsRegisterPriorityCommandTarget;

            var interceptor = new SqlQueryTemplateCommand(package, dte, new TextDocumentService());
            pct.RegisterPriorityCommandTarget(0, interceptor, out _);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return (int)Constants.OLECMDERR_E_NOTSUPPORTED;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            try
            {
                if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
                {
                    switch (nCmdID)
                    {
                        case (uint)VSConstants.VSStd2KCmdID.TAB:

                            var textSelection = _textDocumentService.GetTextDocument().Selection;
                            var activePoint = textSelection.ActivePoint;
                            var edit = activePoint.CreateEditPoint();
                            var currentText = edit.GetLines(activePoint.Line, activePoint.Line + 1);

                            if (currentText.EndsWith("*sel"))
                            {
                                edit.EndOfLine();
                                textSelection.DeleteLeft(4);
                                edit.Insert("SELECT * FROM dbo.tables");
                                edit.EndOfLine();
                                textSelection.DeleteLeft(1);
                            }

                            if (currentText.EndsWith("*s-columns"))
                            {
                                edit.EndOfLine();
                                textSelection.DeleteLeft("*search-columns".Length);
                                edit.Insert("SELECT  \r\n\tc.name  AS 'ColumnName'\r\n\t,t.name AS 'TableName'\r\nFROM sys.columns c\r\nJOIN sys.tables  t   ON c.object_id = t.object_id\r\nWHERE c.name LIKE '%MyName%'\r\nORDER BY TableName, ColumnName");
                                edit.EndOfLine();
                                textSelection.DeleteLeft(1);
                            }

                            break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return (int)Constants.MSOCMDERR_E_FIRST;
        }
    }
}