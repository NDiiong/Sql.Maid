using System.Data;

namespace Sqlserver.maid.Services.File
{
    public interface IFileService
    {
        void WriteFile(string path, DataTable datatable);
    }
}