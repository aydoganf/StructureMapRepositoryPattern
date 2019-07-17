using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Utility;
using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Manager
{
    public class PersonManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        #endregion

        public PersonManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }

        public Person CreateNewPerson(string name, string email)
        {
            return coreContext.New<Person>().With(name, email);
        }

        public List<Person> GetAllPersonList()
        {
            return coreContext.Query<Persons>().GetAll();
        }

        public Person GetPersonById(int id)
        {
            return coreContext.Query<Persons>().GetById(id);
        }

        public void DeletePerson(Person person)
        {
            coreContext.Query<Persons>().Delete(person);
        }

        public void UpdatePerson(Person person)
        {
            coreContext.Query<Persons>().Update(person);
        }

        public void GetPersonInfoTable(Person person)
        {
            ConsoleTablePrinter.PrintObject(person);
        }

        public void GetAllPersonListInfoTable()
        {
            var personList = GetAllPersonList();
            ConsoleTablePrinter.PrintAllDataSet(personList);
        }

        public Person GetPersonByEmail(string email)
        {
            return coreContext.Query<Persons>().GetByEmail(email);
        }
    }
}
