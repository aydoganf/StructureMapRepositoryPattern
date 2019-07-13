using StructureMapRepositoryPattern.Service;
using StructureMapRepositoryPattern.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Repository
{
    public abstract class Query<T> : IRepository<T> where T : IData
    {
        private List<T> _dataList;
        private List<T> dataList
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

        private readonly IJsonReader jsonReader;
        private readonly IDataValidator<T> dataValidator;

        public Query(IJsonReader jsonReader, IDataValidator<T> dataValidator)
        {
            this.jsonReader = jsonReader;
            this.dataValidator = dataValidator;
        }

        public List<T> GetAll()
        {
            return dataList;
        }

        public T GetById(int id)
        {
            return dataList.FirstOrDefault(i => i.Id == id);
        }

        protected virtual void GetDataFromSource()
        {
            _dataList = jsonReader.ReadAllData<T>();
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

        public void Delete(T data)
        {
            dataList.Remove(data);
            jsonReader.WriteFile(_dataList);
        }

        public void Update()
        {
            jsonReader.WriteFile(_dataList);
        }

        public void GetDataFormattedInfo(T obj)
        {
            var props = obj.GetType().GetProperties();
            ConsoleTablePrinter.PrintHeader(props.Select(i => i.Name).ToArray());
            ConsoleTablePrinter.PrintLine();
            ConsoleTablePrinter.PrintRow(props.Select(i => i.GetValue(obj).ToString()).ToArray());
            ConsoleTablePrinter.PrintLine();
        }

        public void GetDataFormattedInfo()
        {
            var props = typeof(T).GetProperties();
            ConsoleTablePrinter.PrintHeader(props.Select(i => i.Name).ToArray());
            ConsoleTablePrinter.PrintLine();

            foreach (var data in dataList)
            {
                ConsoleTablePrinter.PrintRow(props.Select(i => i.GetValue(data).ToString()).ToArray());
                ConsoleTablePrinter.PrintLine();
            }
        }
    }
}
