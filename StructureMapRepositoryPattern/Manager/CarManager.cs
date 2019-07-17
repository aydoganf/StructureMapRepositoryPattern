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
            return coreContext.New<Car>().With(brandName, price);
        }
        
        public List<Car> GetAllCarList()
        {
            return coreContext.Query<Cars>().GetAll();
        }

        public Car GetCarById(int id)
        {
            return coreContext.Query<Cars>().GetById(id);
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
