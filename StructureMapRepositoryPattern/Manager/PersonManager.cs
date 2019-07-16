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
            var person = coreContext.New<Person>().With(name, email);
            return person; //coreContext.Query<Persons>().Insert(person);
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
            //coreContext.Query<Persons>().Delete(person);
        }

        public void UpdatePerson(Person person)
        {
            //coreContext.Query<Persons>().Update(person);
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
    }
}
