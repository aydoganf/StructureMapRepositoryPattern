using StructureMapRepositoryPattern.Service;
using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Utility
{
    public class JsonReader : IJsonReader
    {
        public string JsonPath { get; private set; }

        public JsonReader(string jsonPath)
        {
            JsonPath = jsonPath;
        }

        private string ReadFile()
        {
            return System.IO.File.ReadAllText(JsonPath);
        }

        public List<T> ReadAllData<T>() where T : IData
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(ReadFile());
            return obj ?? new List<T>();
        }

        public void WriteFile<T>(List<T> data) where T : IData
        {
            System.IO.File.WriteAllText(JsonPath, Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
    }
}
