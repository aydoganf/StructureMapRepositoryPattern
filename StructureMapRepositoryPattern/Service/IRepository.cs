using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Service
{
    public interface IRepository<T> where T : IData
    {
        //List<T> GetAll();
        //T GetById(int id);
        T Insert(T data);
        void Delete(T data);
        void Update(T data);
    }
}
