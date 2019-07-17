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

        public Query(ICoreContext coreContext, IJsonReader jsonReader)
        {
            //string appDir = System.AppContext.BaseDirectory;
            //var dataSourceDir = System.IO.Directory.GetParent(appDir).Parent.Parent.Parent;
            //string dataSourceFileName = string.Concat(dataSourceDir.FullName, @"\dataSource\",
            //    typeof(T).Name.ToLower(), "s.json");

            this.jsonReader = jsonReader;
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

        public void Update(T data)
        {
            var d = dataList.FirstOrDefault(i => i.Id == data.Id);
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                prop.SetValue(d, prop.GetValue(data));
            }
            jsonReader.WriteFile(_dataList);
        }

        public void Delete(T data)
        {
            dataList.Remove(GetById(data.Id));
            jsonReader.WriteFile(_dataList);
        }
    }
}
