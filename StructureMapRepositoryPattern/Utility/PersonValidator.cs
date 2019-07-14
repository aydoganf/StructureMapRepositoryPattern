using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Service;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern.Utility
{
    public class PersonValidator : IDataValidator<Person>
    {
        public IEnumerable<string> Validate(Person data, List<Person> dataSource)
        {
            List<string> resultSet = new List<string>();
            if (dataSource.Any(p => p.Email.ToLower() == data.Email.ToLower()))
            {
                resultSet.Add(string.Format("There is already person in repo with the email of: {0}", data.Email));
            }
            return resultSet;
        }
    }
}
