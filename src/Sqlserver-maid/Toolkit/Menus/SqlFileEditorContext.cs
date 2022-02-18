namespace Sqlserver.maid.Toolkit.Menus
{
    public class SqlFileEditorContext : SqlMenuContext, ISqlFileEditorContext
    {
        private const string _fileEditorContextName = "SQL Files Editor Context";
        protected override string ContextName => _fileEditorContextName;
    }
}