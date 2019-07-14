﻿using StructureMapRepositoryPattern.Service;
using StructureMapRepositoryPattern.Utility;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : IData
    {
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

        private readonly IJsonReader jsonReader;
        private readonly IDataValidator<T> dataValidator;

        public Repository(IJsonReader jsonReader, IDataValidator<T> dataValidator)
        {
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