using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Service;
using StructureMapRepositoryPattern.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Repository
{
    public abstract class Query<T> : IQuery where T : IData
    {
        #region IoC
        private readonly IJsonReader jsonReader;
        #endregion

        private List<T> _dataList;
        protected List<T> dataList
        {
            get
            {
                if (_dataList == null || _dataList.Count == 0)
                {
                    GetDataFromSource();
                }
                return _dataList;
            }
        }

        public Query(ICoreContext coreContext)
        {
            string appDir = System.AppContext.BaseDirectory;
            var dataSourceDir = System.IO.Directory.GetParent(appDir).Parent.Parent.Parent;
            string dataSourceFileName = string.Concat(dataSourceDir.FullName, @"\dataSource\",
                typeof(T).Name.ToLower(), "s.json");

            this.jsonReader = new JsonReader(dataSourceFileName);
        }

        private void GetDataFromSource()
        {
            _dataList = jsonReader.ReadAllData<T>();
        }

        public List<T> GetAll()
        {
            return dataList;
        }

        public T GetById(int id)
        {
            return dataList.FirstOrDefault(d => d.Id == id);
        }

        public T GetBy(Func<T, bool> condition)
        {
            return dataList.FirstOrDefault(condition);
        }

        public List<T> GetListBy(Func<T, bool> condition)
        {
            return dataList.Where(condition).ToList();
        }

    }
}
