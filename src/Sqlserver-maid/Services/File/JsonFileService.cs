using Newtonsoft.Json;
using System.Data;
using Formatting = Newtonsoft.Json.Formatting;

namespace Sqlserver.maid.Services.File
{
    public class JsonFileService : IFileService
    {
        public void WriteFile(string path, DataTable datatable)
        {
            var json = JsonConvert.SerializeObject(datatable, Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }
    }
}