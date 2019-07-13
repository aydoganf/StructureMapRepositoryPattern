using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Service
{
    public interface IRepository<T> where T : IData
    {
        List<T> GetAll();
        T GetById(int id);
        T Insert(IData data);
        void Delete(IData data);
        void Update();
    }
}
