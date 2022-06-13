using System.Data;

namespace Sqlserver.maid.Services.File
{
    public interface IFileService
    {
        string AsJson(DataTable datatable);
        void WriteFile(string path, DataTable datatable);
    }
}