using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Utility;
using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Manager
{
    public class CarManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        #endregion

        public CarManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;            
        }

        public Car CreateCar(string brandName, double price)
        {
            var car = coreContext.New<Car>().With(brandName, price);
            return car; //coreContext.Query<Cars>().Insert(car);
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
            //coreContext.Query<Cars>().Delete(car);
        }

        public void UpdateCar(Car car)
        {
            //coreContext.Query<Cars>().Update(car);
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
