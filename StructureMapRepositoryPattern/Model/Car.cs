using StructureMapRepositoryPattern.Service;

namespace StructureMapRepositoryPattern.Model
{
    public class Car : IData
    {
        public Car()
        {
        }

        public int Id { get; set; }
        public string BrandName { get; set; }
        public double Price { get; set; }

        public Car With(string brandName, double price)
        {
            BrandName = brandName;
            Price = price;
            return this;
        }

        public Car Save(string brandName, double price)
        {
            BrandName = brandName;
            Price = price;
            return this;
        }
    }
}
