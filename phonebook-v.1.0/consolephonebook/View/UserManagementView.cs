using ConsolePhonebook.Entity;
using ConsolePhonebook.Repository;
using ConsolePhonebook.Service;
using ConsolePhonebook.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhonebook.View
{
    public class UserManagementView
    {
        public Action Show()
        {
            while (true)
            {
                UserManagementEnum choice = RenderMenu();
                
                var dict = new Dictionary<UserManagementEnum, Action>
                {
                    { UserManagementEnum.Select, (() => GetAll()) },
                    { UserManagementEnum.View, (() => View()) },
                    { UserManagementEnum.Insert, (() => Add()) },
                    { UserManagementEnum.Update, (() => Update()) },
                    { UserManagementEnum.Delete, (() => Delete()) },
                    {UserManagementEnum.Exit, (() => Exit()) }
                };

                try
                {
                    dict[choice].Invoke();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                }
            }
        }
    
        private UserManagementEnum RenderMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Users management:");
                Console.WriteLine("[G]et all Users");
                Console.WriteLine("[V]iew User");
                Console.WriteLine("[A]dd User");
                Console.WriteLine("[E]dit User");
                Console.WriteLine("[D]elete User");
                Console.WriteLine("E[x]it");

                string choice = Console.ReadLine().ToUpper();
                var list = new List<KeyValuePair<string, UserManagementEnum>>
                {
                    new KeyValuePair<string, UserManagementEnum>("G", UserManagementEnum.Select),
                    new KeyValuePair<string, UserManagementEnum>("V", UserManagementEnum.View),
                    new KeyValuePair<string, UserManagementEnum>("A", UserManagementEnum.Insert),
                    new KeyValuePair<string, UserManagementEnum>("E", UserManagementEnum.Update),
                    new KeyValuePair<string, UserManagementEnum>("D", UserManagementEnum.Delete),
                    new KeyValuePair<string, UserManagementEnum>("X", UserManagementEnum.Exit)
                };

                foreach (var pair in list)
                {
                    if (pair.Key == choice)
                    {
                        return pair.Value;
                    }
                }
                Console.WriteLine("Invalid choice.");
                Console.ReadKey(true);
            }
        }

        private void GetAll()
        {
            Console.Clear();

            UsersRepository usersRepository = new UsersRepository("users.txt");
            List<User> users = usersRepository.GetAll();

            foreach (User user in users)
            {
                Console.WriteLine("ID:" + user.Id);
                Console.WriteLine("Username :" + user.Username);
                Console.WriteLine("Password :" + user.Password);
                Console.WriteLine("First Name :" + user.FirstName);
                Console.WriteLine("Last Name :" + user.LastName);
                Console.WriteLine("Is Admin:" + user.IsAdmin);

                Console.WriteLine("########################################");
            }

            Console.ReadKey(true);
        }

        private void View()
        {
            Console.Clear();

            Console.Write("User ID: ");
            int userId = Convert.ToInt32(Console.ReadLine());

            UsersRepository usersRepository = new UsersRepository("users.txt");

            User user = usersRepository.GetById(userId);
            if (user == null)
            {
                Console.Clear();
                Console.WriteLine("User not found.");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("ID:" + user.Id);
            Console.WriteLine("Username :" + user.Username);
            Console.WriteLine("Password :" + user.Password);
            Console.WriteLine("First Name :" + user.FirstName);
            Console.WriteLine("Last Name :" + user.LastName);
            Console.WriteLine("Is Admin:" + user.IsAdmin);

            Console.ReadKey(true);
        }

        private void Add()
        {
            Console.Clear();

            User user = new User();

            Console.WriteLine("Add new User:");

            Console.Write("Username: ");
            user.Username = Console.ReadLine();

            Console.Write("Password: ");
            user.Password = Console.ReadLine();

            Console.Write("First Name: ");
            user.FirstName = Console.ReadLine();

            Console.Write("Last Name: ");
            user.LastName = Console.ReadLine();

            Console.Write("Is Admin (True/False): ");
            user.IsAdmin = Convert.ToBoolean(Console.ReadLine());

            UsersRepository usersRepository = new UsersRepository("users.txt");
            usersRepository.Save(user);

            Console.WriteLine("User saved successfully.");
            Console.ReadKey(true);
        }

        private void Update()
        {
            Console.Clear();

            Console.Write("User ID: ");
            int userId = Convert.ToInt32(Console.ReadLine());

            UsersRepository usersRepository = new UsersRepository("users.txt");
            User user = usersRepository.GetById(userId);


            if (user == null)
            {
                Console.Clear();
                Console.WriteLine("User not found.");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Editing User (" + user.Username + ")");
            Console.WriteLine("ID:" + user.Id);

            Console.WriteLine("Username :" + user.Username);
            Console.Write("New Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Password :" + user.Password);
            Console.Write("New Password:");
            string password = Console.ReadLine();

            Console.WriteLine("First Name :" + user.FirstName);
            Console.Write("New First Name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Last Name :" + user.LastName);
            Console.Write("New Last Name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Is Admin :" + user.IsAdmin);
            Console.Write("New Is Admin (True/False):");
            string isAdmin = Console.ReadLine();

            user.Username = getAppendString(user.Username, username);
            user.Password = getAppendString(user.Password, password);
            user.FirstName = getAppendString(user.FirstName, firstName);
            user.LastName = getAppendString(user.LastName, lastName);


            //if (!string.IsNullOrEmpty(username))
            //    user.Username = username;

            //if (!string.IsNullOrEmpty(password))
            //    user.Password = password;
            //if (!string.IsNullOrEmpty(firstName))
            //    user.FirstName = firstName;
            //if (!string.IsNullOrEmpty(lastName))
            //    user.LastName = lastName;
            if (!string.IsNullOrEmpty(isAdmin))
                user.IsAdmin = Convert.ToBoolean(isAdmin);

            usersRepository.Save(user);

            Console.WriteLine("User saved successfully.");
            Console.ReadKey(true);
        }

        private String getAppendString(String value, String appendString)
        {
            if (appendString == null)
            {
                return value;
            }
            return appendString;
        }

        private void Delete()
        {
            UsersRepository usersRepository = new UsersRepository("users.txt");

            Console.Clear();

            Console.WriteLine("Delete User:");
            Console.Write("User Id: ");
            int userId = Convert.ToInt32(Console.ReadLine());

            User user = usersRepository.GetById(userId);
            if (user == null)
            {
                Console.WriteLine("User not found!");
            }
            else
            {
                usersRepository.Delete(user);
                Console.WriteLine("User deleted successfully.");
            }
            Console.ReadKey(true);
        }
        
        private void Exit()
        {
            new AdminView().Show();
            return;
        }
    }
}
