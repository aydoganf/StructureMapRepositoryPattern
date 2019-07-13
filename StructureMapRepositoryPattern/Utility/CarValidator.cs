using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Service;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Utility
{
    public class CarValidator : IDataValidator<Car>
    {
        public CarValidator()
        {
        }

        public IEnumerable<string> Validate(Car data, List<Car> dataSource)
        {
            List<string> resultSet = new List<string>();
            if (dataSource.Any(c => c.BrandName.ToLower() == data.BrandName.ToLower()))
            {
                resultSet.Add(string.Format("There is already a car in repo with the name of: {0}", data.BrandName));
            }
            return resultSet;
        }

    }
}
