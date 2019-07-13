using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Service
{
    public interface IJsonReader
    {
        string JsonPath { get; }
        void WriteFile<T>(List<T> data) where T : IData;
        List<T> ReadAllData<T>() where T : IData;
    }
}
