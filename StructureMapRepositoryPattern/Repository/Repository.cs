using StructureMapRepositoryPattern.Service;
using StructureMapRepositoryPattern.Utility;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Repository
{
    public class Repository<T> : IRepository<T> where T : IData
    {
        #region IoC
        private readonly IJsonReader jsonReader;
        private readonly IDataValidator<T> dataValidator;
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

        public Repository(IJsonReader jsonReader, IDataValidator<T> dataValidator)
        {
            //string appDir = System.AppContext.BaseDirectory;
            //var dataSourceDir = System.IO.Directory.GetParent(appDir).Parent.Parent.Parent;
            //string dataSourceFileName = string.Concat(dataSourceDir.FullName, @"\dataSource\",
            //    typeof(T).Name.ToLower(), "s.json");
            this.jsonReader = jsonReader;
            this.dataValidator = dataValidator;
        }

        private void GetDataFromSource()
        {
            _dataList = jsonReader.ReadAllData<T>();
        }

        public void Delete(T data)
        {
            dataList.Remove(dataList.FirstOrDefault(d => d.Id == data.Id));
            jsonReader.WriteFile(_dataList);
        }

        public T Insert(T data)
        {
            var validatorResult = dataValidator.Validate(data, dataList);
            if (validatorResult.Count() != 0)
            {
                throw new DataAlreadyExistException(validatorResult.First());
            }

            //..
            int id = 1;
            if (dataList.Count != 0)
            {
                id = dataList.Max(i => i.Id) + 1;
            }
            data.Id = id;
            _dataList.Add(data);
            jsonReader.WriteFile(_dataList);
            return data;
        }

        public void Update(T data)
        {
            var d = dataList.FirstOrDefault(i => i.Id == data.Id);
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                prop.SetValue(d, prop.GetValue(data));
            }
            //this._dataList = dataList;
            jsonReader.WriteFile(_dataList);
        }
    }
}
