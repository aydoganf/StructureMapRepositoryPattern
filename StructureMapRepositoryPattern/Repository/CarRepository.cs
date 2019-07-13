using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Service;

namespace StructureMapRepositoryPattern.Repository
{
    public class CarRepository : Query<Car>
    {
        public CarRepository(IJsonReader jsonReader, IDataValidator<Car> dataValidator) : base(jsonReader, dataValidator)
        {
        }
    }
}
