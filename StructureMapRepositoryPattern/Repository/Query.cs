using StructureMapRepositoryPattern.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Repository
{
    public abstract class Query<T> : Repository<T>, IQuery where T : IData
    {
        public Query(IJsonReader jsonReader, IDataValidator<T> dataValidator) : base(jsonReader, dataValidator)
        {
        }

        public List<T> GetAll()
        {
            return dataList;
        }

        public T GetById(int id)
        {
            return dataList.FirstOrDefault(i => i.Id == id);
        }

        public List<T> GetListBy(Func<T, bool> predicate)
        {
            return dataList.Where(predicate).ToList();
        }

        public T GetSingleBy(Func<T, bool> predicate)
        {
            return dataList.FirstOrDefault(predicate);
        }
    }
}
