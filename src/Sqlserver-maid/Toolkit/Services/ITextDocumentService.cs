using EnvDTE;

namespace Sqlserver.maid.Toolkit.Services
{
    public interface ITextDocumentService
    {
        void Collapse(string text);
        Document GetActiveDocument();
        TextDocument GetTextDocument();
        TextSelection GetTextSelection();
    }
}