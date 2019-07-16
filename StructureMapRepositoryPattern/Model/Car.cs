using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Service;

namespace StructureMapRepositoryPattern.Model
{
    public class Car : IData
    {
        #region IoC
        private readonly IRepository<Car> repository;
        #endregion

        public Car(IRepository<Car> repository)
        {
            this.repository = repository;
        }

        public int Id { get; set; }
        public string BrandName { get; set; }
        public double Price { get; set; }

        public Car With(string brandName, double price)
        {
            BrandName = brandName;
            Price = price;
            repository.Insert(this);
            return this;
        }

        public Car Save(string brandName, double price)
        {
            BrandName = brandName;
            Price = price;
            return this;
        }
    }

    public class Cars : Query<Car>
    {
        public Cars(ICoreContext coreContext) : base(coreContext)
        {
        }
    }
}
