using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMapRepositoryPattern.Context;
using StructureMapRepositoryPattern.Manager;
using StructureMapRepositoryPattern.Model;
using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Service;
using StructureMapRepositoryPattern.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureMapRepositoryPattern
{
    public class RepositoryPolicy : ConfiguredInstancePolicy
    {
        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            if (instance.PluggedType.BaseType.IsGenericType &&
                instance.PluggedType.BaseType.GetGenericTypeDefinition() == typeof(Query<>))
            {
                string appDir = System.AppContext.BaseDirectory;
                var dataSourceDir = System.IO.Directory.GetParent(appDir).Parent.Parent.Parent;
                string dataSourceFileName = string.Concat(dataSourceDir.FullName, @"\dataSource\",
                    pluginType.Name.ToLower(), ".json");

                var param = instance.Constructor
                    .GetParameters()
                    .FirstOrDefault(p => p.ParameterType == typeof(IJsonReader));
                if (param != null)
                {
                    var jsonReader = new JsonReader(dataSourceFileName);
                    instance.Dependencies.AddForConstructorParameter(param, jsonReader);
                }
            }

            if (instance.PluggedType.IsGenericType &&
                instance.PluggedType.GetGenericTypeDefinition() == typeof(Repository<>))
            {
                string appDir = System.AppContext.BaseDirectory;
                var dataSourceDir = System.IO.Directory.GetParent(appDir).Parent.Parent.Parent;
                string dataSourceFileName = string.Concat(dataSourceDir.FullName, @"\dataSource\",
                    instance.PluggedType.GetGenericArguments().First().Name.ToLower(), "s.json");

                var param = instance.Constructor
                    .GetParameters()
                    .FirstOrDefault(p => p.ParameterType == typeof(IJsonReader));
                if (param != null)
                {
                    var jsonReader = new JsonReader(dataSourceFileName);
                    instance.Dependencies.AddForConstructorParameter(param, jsonReader);
                }
            }
        }
    }

    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For(typeof(IRepository<>)).Use(typeof(Repository<>));
        }
    }

    class Program
    {
        static readonly string GET_PERSON_LIST = "persons";
        static readonly string ADD_PERSON = "add person";
        static readonly string GET_PERSON = "get person";
        static readonly string DELETE_PERSON = "delete person";
        static readonly string UPDATE_PERSON = "update person";
        static readonly string GET_PERSON_EMAIL = "get person email";

        static readonly string GET_CAR_LIST = "cars";
        static readonly string ADD_CAR = "add car";
        static readonly string GET_CAR = "get car";
        static readonly string DELETE_CAR = "delete car";
        static readonly string UPDATE_CAR = "update car";

        static List<string> personCommands = new List<string>()
            {
                GET_PERSON_LIST,
                ADD_PERSON,
                GET_PERSON,
                DELETE_PERSON,
                UPDATE_PERSON,
                GET_PERSON_EMAIL
            };

        static List<string> carCommands = new List<string>()
            {
                GET_CAR_LIST,
                ADD_CAR,
                GET_CAR,
                DELETE_CAR,
                UPDATE_CAR
            };

        static void Main(string[] args)
        {
            var contaier = new Container(_ =>
            {
                _.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.WithDefaultConventions();
                    s.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
                    s.ConnectImplementationsToTypesClosing(typeof(IDataValidator<>));
                    s.ConnectImplementationsToTypesClosing(typeof(Query<>));
                });
                _.Policies.Add<RepositoryPolicy>();
                _.AddRegistry<RepositoryRegistry>();
            });


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("About Program");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("To stop program, give 'exit' command anywhere.");
            Console.WriteLine("----------------------------------------------");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();

            var carManager = contaier.GetInstance<CarManager>();
            var personManager = contaier.GetInstance<PersonManager>();

            string mainCommand = "go";
            string dataSelector = "";

            while (mainCommand == "go")
            {
                string carCommand = "cars";
                string personCommand = "persons";

                if (dataSelector != "person" && dataSelector != "car")
                    ShowDataSelectorCommand(ref dataSelector, ref personCommand, ref carCommand);

                while (dataSelector == "car")
                {
                    while (carCommands.Contains(carCommand))
                    {
                        var allCars = new List<Car>();

                        if (carCommand == GET_CAR_LIST)
                        {
                            allCars = carManager.GetAllCarList(); //carRepo.GetAll();
                            Console.WriteLine("there are {0} car(s) in repo.", allCars.Count);
                            Console.WriteLine();
                            carManager.GetAllCarListInfoTable();
                            //carRepo.GetDataFormattedInfo();
                            Console.WriteLine();
                            Console.WriteLine();
                        }

                        if (carCommand == ADD_CAR)
                        {
                            Console.WriteLine("You are trying to add car.. So, give me the brand name of the car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            string carBrandName = Console.ReadLine();
                            Console.ResetColor();
                            Console.WriteLine("Give me the price of the car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            double carPrice = Convert.ToDouble(Console.ReadLine());
                            Console.ResetColor();

                            try
                            {
                                carManager.CreateCar(carBrandName, carPrice); //carRepo.Insert(car);
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car added into repo succesfully");
                            }
                            catch (Exception ex)
                            {
                                ShowErrorMessage(ex.Message);
                            }

                            allCars = carManager.GetAllCarList(); //carRepo.GetAll();
                            Console.WriteLine("there are {0} car(s) in repo now..", allCars.Count);
                            Console.WriteLine();
                            carManager.GetAllCarListInfoTable(); //car.GetDataFormattedInfo();
                            Console.WriteLine();
                            Console.WriteLine();
                        }

                        if (carCommand == GET_CAR)
                        {
                            Console.WriteLine("Give me the id of car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int carId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var car = carManager.GetCarById(carId); //carRepo.GetById(carId);
                            if (car != null)
                            {
                                carManager.GetCarInfoTable(car); //carRepo.GetDataFormattedInfo(car);
                            }
                            else
                            {
                                ShowErrorMessage("Car is not found in repo with the given identifier!");
                            }
                        }

                        if (carCommand == DELETE_CAR)
                        {
                            Console.WriteLine("Give me the id of car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int carId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var car = carManager.GetCarById(carId); //carRepo.GetById(carId);
                            if (car != null)
                            {
                                carManager.DeleteCar(car); //carRepo.Delete(car);
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is deleted from repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                ShowErrorMessage("Car is not found in repo with the given identifier!");
                            }
                        }

                        if (carCommand == UPDATE_CAR)
                        {
                            Console.WriteLine("Give me the id of car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int carId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var car = carManager.GetCarById(carId); //carRepo.GetById(carId);
                            if (car != null)
                            {
                                carManager.GetCarInfoTable(car); //carRepo.GetDataFormattedInfo(car);
                                Console.WriteLine("You are trying to update car.. So, give me the new brand name of the car:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                string carBrandName = Console.ReadLine();
                                Console.ResetColor();
                                Console.WriteLine("Give me the new price of the car:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                double carPrice = Convert.ToDouble(Console.ReadLine());
                                Console.ResetColor();

                                car.Save(carBrandName, carPrice);
                                carManager.UpdateCar(car); //carRepo.Update();

                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is updated in repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                ShowErrorMessage("Car is not found in repo with the given identifier!");
                            }
                        }

                        ShowCarCommand(ref carCommand);
                    }

                    if (!carCommands.Any(i => i == carCommand))
                    {
                        ShowDataSelectorCommand(ref dataSelector, ref personCommand, ref carCommand);
                    }
                }

                while (dataSelector == "person")
                {
                    while (personCommands.Contains(personCommand))
                    {
                        var allPersons = new List<Person>();

                        if (personCommand == GET_PERSON_LIST)
                        {
                            allPersons = personManager.GetAllPersonList(); //personRepo.GetAll();
                            Console.WriteLine("there are {0} person(s) in repo.", allPersons.Count);
                            Console.WriteLine();
                            personManager.GetAllPersonListInfoTable(); //personRepo.GetDataFormattedInfo();
                            Console.WriteLine();
                            Console.WriteLine();
                        }

                        if (personCommand == ADD_PERSON)
                        {
                            Console.WriteLine("You are trying to add person.. So, give me the name of the person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            string personName = Console.ReadLine();
                            Console.ResetColor();
                            Console.WriteLine("Give me the email of the person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            string personEmail = Console.ReadLine();
                            Console.ResetColor();

                            try
                            {
                                var person = personManager.CreateNewPerson(personName, personEmail); //personRepo.Insert(person);
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person added into repo succesfully");
                            }
                            catch (Exception ex)
                            {
                                ShowErrorMessage(ex.Message);
                            }

                            allPersons = personManager.GetAllPersonList();
                            Console.WriteLine("there are {0} person(s) in repo now..", allPersons.Count);
                            Console.WriteLine();
                            personManager.GetAllPersonListInfoTable();
                            Console.WriteLine();
                            Console.WriteLine();
                        }

                        if (personCommand == GET_PERSON)
                        {
                            Console.WriteLine("Give me the id of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int personId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var person = personManager.GetPersonById(personId); //personRepo.GetById(personId);
                            if (person != null)
                            {
                                personManager.GetPersonInfoTable(person); //personRepo.GetDataFormattedInfo(person);
                            }
                            else
                            {
                                ShowErrorMessage("Person is not found in repo with the given identifier!");
                            }
                        }

                        if (personCommand == DELETE_PERSON)
                        {
                            Console.WriteLine("Give me the id of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int personId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var person = personManager.GetPersonById(personId); //personRepo.GetById(personId);
                            if (person != null)
                            {
                                personManager.DeletePerson(person); //personRepo.Delete(person);
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is deleted from repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                ShowErrorMessage("Person is not found in repo with the given identifier!");
                            }
                        }

                        if (personCommand == UPDATE_PERSON)
                        {
                            Console.WriteLine("Give me the id of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int personId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var person = personManager.GetPersonById(personId); //personRepo.GetById(personId);
                            if (person != null)
                            {
                                personManager.GetPersonInfoTable(person); //personRepo.GetDataFormattedInfo(person);
                                Console.WriteLine("You are trying to update person.. So, give me the new name of the person:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                string personName = Console.ReadLine();
                                Console.ResetColor();
                                Console.WriteLine("Give me the new email of the person:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                string personEmail = Console.ReadLine();
                                Console.ResetColor();

                                person.Save(personName, personEmail);
                                personManager.UpdatePerson(person); //personRepo.Update();
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is updated in repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                ShowErrorMessage("Person is not found in repo with the given identifier!");
                            }
                        }

                        if (personCommand == GET_PERSON_EMAIL)
                        {
                            Console.WriteLine("Give me the email of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            string personEmail = Console.ReadLine();
                            Console.ResetColor();

                            var person = personManager.GetPersonByEmail(personEmail);
                            if (person != null)
                            {
                                personManager.GetPersonInfoTable(person);
                            }
                            else
                            {
                                ShowErrorMessage("Person is not found in repo with the given identifier!");
                            }
                        }

                        ShowPersonCommand(ref personCommand);
                    }

                    if (!personCommands.Any(i => i == personCommand))
                    {
                        ShowDataSelectorCommand(ref dataSelector, ref personCommand, ref carCommand);
                    }
                }

                if (dataSelector != "person" && dataSelector != "car")
                {
                    if (dataSelector == "exit")
                        break;

                    Console.WriteLine("If you want to proceed, give 'go' command:");
                    mainCommand = Console.ReadLine();
                }
            }

        }

        public static void ShowDataSelectorCommand(ref string dataSelector, ref string personCommand, ref string carCommand)
        {
            if (personCommand == "exit" || carCommand == "exit")
            {
                dataSelector = "exit";
                return;
            }

            Console.WriteLine("To select data  type, give 'car' or 'person' command:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            dataSelector = Console.ReadLine();
            Console.ResetColor();
        }

        public static void ShowCarCommand(ref string carCommand)
        {
            Console.WriteLine("Here is all commands you could use: (Want to go main data selection, give anything else)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            carCommands.ForEach(c => Console.WriteLine("> {0}", c));
            carCommand = Console.ReadLine();
            Console.ResetColor();
        }

        public static void ShowPersonCommand(ref string personCommand)
        {
            Console.WriteLine("Here is all commands you could use: (Want to go main data selection, give anything else)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            personCommands.ForEach(c => Console.WriteLine("- {0}", c));
            personCommand = Console.ReadLine();
            Console.ResetColor();
        }

        public static void ShowErrorMessage(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
