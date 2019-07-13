using StructureMap;
using StructureMap.Pipeline;
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
                    pluginType.Name.ToLower().Replace("repository", ""), ".json");

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

    class Program
    {
        static readonly string GET_PERSON_LIST = "persons";
        static readonly string ADD_PERSON = "add person";
        static readonly string GET_PERSON = "get person";
        static readonly string DELETE_PERSON = "delete person";
        static readonly string UPDATE_PERSON = "update person";

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
                UPDATE_PERSON
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

                });
                _.Policies.Add<RepositoryPolicy>();
            });

            var carRepo = contaier.GetInstance<CarRepository>();
            var personRepo = contaier.GetInstance<PersonRepository>();

            string mainCommand = "go";
            string dataSelector = "";

            while (mainCommand == "go")
            {
                string carCommand = "cars";
                string personCommand = "persons";

                if (dataSelector != "person" && dataSelector != "car")
                    ShowDataSelectorCommand(ref dataSelector);

                while (dataSelector == "car")
                {
                    while (carCommands.Contains(carCommand))
                    {
                        var allCars = new List<Car>();

                        if (carCommand == GET_CAR_LIST)
                        {
                            allCars = carRepo.GetAll();
                            Console.WriteLine("there are {0} car(s) in repo.", allCars.Count);
                            Console.WriteLine();
                            carRepo.GetDataFormattedInfo();
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

                            var car = contaier.GetInstance<Car>();
                            car.With(carBrandName, carPrice);

                            try
                            {
                                carRepo.Insert(car);
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car added into repo succesfully");
                            }
                            catch (Exception ex)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(ex.Message);
                            }
                            finally { Console.ResetColor(); }

                            allCars = carRepo.GetAll();
                            Console.WriteLine("there are {0} car(s) in repo now..", allCars.Count);
                            Console.WriteLine();
                            carRepo.GetDataFormattedInfo();
                            Console.WriteLine();
                            Console.WriteLine();
                        }

                        if (carCommand == GET_CAR)
                        {
                            Console.WriteLine("Give me the id of car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int carId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var car = carRepo.GetById(carId);
                            if (car != null)
                            {
                                carRepo.GetDataFormattedInfo(car);
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is not found in repo with the given identifier!");
                                Console.ResetColor();
                            }
                        }

                        if (carCommand == DELETE_CAR)
                        {
                            Console.WriteLine("Give me the id of car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int carId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var car = carRepo.GetById(carId);
                            if (car != null)
                            {
                                carRepo.Delete(car);
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is deleted from repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is not found in repo with the given identifier!");
                                Console.ResetColor();
                            }
                        }

                        if (carCommand == UPDATE_CAR)
                        {
                            Console.WriteLine("Give me the id of car:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int carId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var car = carRepo.GetById(carId);
                            if (car != null)
                            {
                                carRepo.GetDataFormattedInfo(car);
                                Console.WriteLine("You are trying to update car.. So, give me the new brand name of the car:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                string carBrandName = Console.ReadLine();
                                Console.ResetColor();
                                Console.WriteLine("Give me the new price of the car:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                double carPrice = Convert.ToDouble(Console.ReadLine());
                                Console.ResetColor();

                                car.Save(carBrandName, carPrice);
                                carRepo.Update();

                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is updated in repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Car is not found in repo with the given identifier!");
                                Console.ResetColor();
                            }
                        }

                        ShowCarCommand(ref carCommand);
                    }

                    if (!carCommands.Any(i => i == carCommand))
                    {
                        ShowDataSelectorCommand(ref dataSelector);
                    }
                }

                while (dataSelector == "person")
                {
                    while (personCommands.Contains(personCommand))
                    {
                        var allPersons = new List<Person>();

                        if (personCommand == GET_PERSON_LIST)
                        {
                            allPersons = personRepo.GetAll();
                            Console.WriteLine("there are {0} person(s) in repo.", allPersons.Count);
                            Console.WriteLine();
                            personRepo.GetDataFormattedInfo();
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

                            var person = contaier.GetInstance<Person>();
                            person.With(personName, personEmail);

                            try
                            {
                                personRepo.Insert(person);
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person added into repo succesfully");
                            }
                            catch (Exception ex)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(ex.Message);
                            }
                            finally { Console.ResetColor(); }

                            allPersons = personRepo.GetAll();
                            Console.WriteLine("there are {0} person(s) in repo now..", allPersons.Count);
                            Console.WriteLine();
                            personRepo.GetDataFormattedInfo();
                            Console.WriteLine();
                            Console.WriteLine();

                            //ShowPersonCommand(ref personCommand);
                        }

                        if (personCommand == GET_PERSON)
                        {
                            Console.WriteLine("Give me the id of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int personId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var person = personRepo.GetById(personId);
                            if (person != null)
                            {
                                personRepo.GetDataFormattedInfo(person);
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is not found in repo with the given identifier!");
                                Console.ResetColor();
                            }
                        }

                        if (personCommand == DELETE_PERSON)
                        {
                            Console.WriteLine("Give me the id of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int personId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var person = personRepo.GetById(personId);
                            if (person != null)
                            {
                                personRepo.Delete(person);
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is deleted from repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is not found in repo with the given identifier!");
                                Console.ResetColor();
                            }
                        }

                        if (personCommand == UPDATE_PERSON)
                        {
                            Console.WriteLine("Give me the id of person:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            int personId = Convert.ToInt32(Console.ReadLine());
                            Console.ResetColor();

                            var person = personRepo.GetById(personId);
                            if (person != null)
                            {
                                personRepo.GetDataFormattedInfo(person);
                                Console.WriteLine("You are trying to update person.. So, give me the new name of the person:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                string personName = Console.ReadLine();
                                Console.ResetColor();
                                Console.WriteLine("Give me the new email of the person:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                string personEmail = Console.ReadLine();
                                Console.ResetColor();

                                person.Save(personName, personEmail);
                                personRepo.Update();
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is updated in repo succesfully.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Person is not found in repo with the given identifier!");
                                Console.ResetColor();
                            }
                            ShowPersonCommand(ref personCommand);
                        }

                        ShowPersonCommand(ref personCommand);
                    }

                    if (!personCommands.Any(i => i == personCommand))
                    {
                        ShowDataSelectorCommand(ref dataSelector);
                    }
                }

                if (dataSelector != "person" && dataSelector != "car")
                {
                    Console.WriteLine("If you want to proceed, give 'go' command:");
                    mainCommand = Console.ReadLine();
                }
            }

        }

        public static void ShowDataSelectorCommand(ref string dataSelector)
        {
            Console.WriteLine("To select data  type, give 'car' or 'person' command:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            dataSelector = Console.ReadLine();
            Console.ResetColor();
        }

        public static void ShowCarCommand(ref string carCommand)
        {
            Console.WriteLine("Here is all commands you could use:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            carCommands.ForEach(c => Console.WriteLine("> {0}", c));
            carCommand = Console.ReadLine();
            Console.ResetColor();
        }

        public static void ShowPersonCommand(ref string personCommand)
        {
            Console.WriteLine("Here is all commands you could use:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            personCommands.ForEach(c => Console.WriteLine("- {0}", c));
            personCommand = Console.ReadLine();
            Console.ResetColor();
        }
    }
}
