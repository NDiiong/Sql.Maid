﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using Formatting = Newtonsoft.Json.Formatting;

namespace Sqlserver.maid.Services.File
{
    public interface IJsonService : IFileService
    {
        string AsJson(DataTable datatable);
    }

    public class JsonFileService : IFileService, IJsonService
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public string AsJson(DataTable datatable)
        {
            return JsonConvert.SerializeObject(datatable, Formatting.Indented, _jsonSerializerSettings);
        }

        public void WriteFile(string path, DataTable datatable)
        {
            var json = JsonConvert.SerializeObject(datatable, Formatting.Indented, _jsonSerializerSettings);
            System.IO.File.WriteAllText(path, json);
        }
    }
}