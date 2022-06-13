namespace Sqlserver.maid.Services.Runtime
{
    public interface IClipboardService
    {
        void Set(string @value);
        string GetFromClipboard();
    }
}