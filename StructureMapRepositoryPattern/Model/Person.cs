using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Service;
using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Model
{
    public class Person : IData
    {
        #region IoC
        private readonly IRepository<Person> repository;
        #endregion

        public Person(IRepository<Person> repository)
        {
            this.repository = repository;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Person With(string name, string email)
        {
            Name = name;
            Email = email;
            repository.Insert(this);
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
        public Persons(ICoreContext coreContext, IJsonReader jsonReader) : base(coreContext, jsonReader)
        {
        }

        public Person GetByEmail(string email)
        {
            return base.GetBy(p => p.Email == email);
        }

        public List<Person> GetListByName(string name)
        {
            return base.GetListBy(p => p.Name == name);
        }
    }
}
