using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using Formatting = Newtonsoft.Json.Formatting;

namespace Sqlserver.maid.Services.File
{
    public class JsonFileService : IFileService
    {
        public void WriteFile(string path, DataTable datatable)
        {
            var json = JsonConvert.SerializeObject(datatable, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });

            System.IO.File.WriteAllText(path, json);
        }
    }
}