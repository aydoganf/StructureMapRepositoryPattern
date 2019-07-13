using StructureMap;
using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Service;

namespace StructureMapRepositoryPattern.Repository
{
    public class PersonRepository : Query<Person>
    {
        public PersonRepository(IJsonReader jsonReader, IDataValidator<Person> dataValidator) : base(jsonReader, dataValidator)
        {
        }
    }
}
