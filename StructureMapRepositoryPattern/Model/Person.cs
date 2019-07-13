using StructureMapRepositoryPattern.Service;

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
}
