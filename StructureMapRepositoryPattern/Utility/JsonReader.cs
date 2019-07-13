using StructureMapRepositoryPattern.Service;
using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Utility
{
    public class JsonReader : IJsonReader
    {
        private readonly string jsonPath;

        public JsonReader(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        private string ReadFile()
        {
            return System.IO.File.ReadAllText(jsonPath);
        }

        public List<T> ReadAllData<T>() where T : IData
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(ReadFile());
            return obj ?? new List<T>();
        }

        public void WriteFile<T>(List<T> data) where T : IData
        {
            System.IO.File.WriteAllText(jsonPath, Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
    }
}
