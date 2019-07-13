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

        public T Insert(IData data)
        {
            var validatorResult = dataValidator.Validate((T)data, dataList);
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
            _dataList.Add((T)data);
            jsonReader.WriteFile(_dataList);
            return (T)data;
        }

        public void Delete(IData data)
        {
            dataList.Remove((T)data);
            jsonReader.WriteFile(_dataList);
        }

        public void Update()
        {
            jsonReader.WriteFile(_dataList);
        }

        public void GetDataFormattedInfo(T obj)
        {
            var props = obj.GetType().GetProperties();
            Extensions.PrintHeader(props.Select(i => i.Name).ToArray());
            Extensions.PrintLine();
            Extensions.PrintRow(props.Select(i => i.GetValue(obj).ToString()).ToArray());
            Extensions.PrintLine();
        }

        public void GetDataFormattedInfo()
        {
            var props = typeof(T).GetProperties();
            Extensions.PrintHeader(props.Select(i => i.Name).ToArray());
            Extensions.PrintLine();

            foreach (var data in dataList)
            {
                Extensions.PrintRow(props.Select(i => i.GetValue(data).ToString()).ToArray());
                Extensions.PrintLine();
            }
        }
    }

    public static class Extensions
    {
        public static int tableWidth = 77;

        public static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        public static void PrintHeader(params string[] headers)
        {
            int width = (tableWidth - headers.Length) / headers.Length;
            string row = "|";

            foreach (string header in headers)
            {
                row += AlignCentre(header, width) + "|";
            }

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(row);
            Console.ResetColor();
        }

        public static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
