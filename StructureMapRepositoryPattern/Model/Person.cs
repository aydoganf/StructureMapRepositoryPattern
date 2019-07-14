using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Service;
using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Model
{
    public class Person : IData
    {
        public Person()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Person With(string name, string email)
        {
            Name = name;
            Email = email;
            return this;
        }

        public Person Save(string name, string email)
        {
            Name = name;
            Email = email;
            return this;
        }
    }

    public class Persons : Query<Person>
    {
        public Persons(IJsonReader jsonReader, IDataValidator<Person> dataValidator) : base(jsonReader, dataValidator)
        {
        }

        public Person GetByEmail(string email)
        {
            return base.GetSingleBy(p => p.Email == email);
        }

        public List<Person> GetListByName(string name)
        {
            return base.GetListBy(p => p.Name == name);
        }
    }
}
