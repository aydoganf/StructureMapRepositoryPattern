using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureMapRepositoryPattern.Manager
{
    public class CarManager
    {
        private readonly ICoreContext coreContext;


        public CarManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;            
        }

        public Car CreateCar(string brandName, double price)
        {
            var car = coreContext.New<Car>().With(brandName, price);
            return coreContext.Query<Cars>().Insert(car);
        }
        
        public List<Car> GetAllCarList()
        {
            return coreContext.New<Cars>().GetAll();
        }

        public Car GetCarById(int id)
        {
            return coreContext.New<Cars>().GetById(id);
        }

        public void DeleteCar(Car car)
        {
            coreContext.Query<Cars>().Delete(car);
        }

        public void UpdateCar(Car car)
        {
            coreContext.Query<Cars>().Update(car);
        }

        public void GetCarInfoTable(Car car)
        {
            ConsoleTablePrinter.PrintObject(car);
        }

        public void GetAllCarListInfoTable()
        {
            var carList = GetAllCarList();
            ConsoleTablePrinter.PrintAllDataSet(carList);
        }
    }
}
