using System;
using CommonLibrary.database;

namespace DebugConsoleProject
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            User user = new User(0, "123", "4", User.UserAccessLevel.Admin);
            Console.WriteLine(user.IsCorrectPassword("4"));

            Database<User> b = Database<User>.Read("1.txt");
            b.Add(user);
            b.Write("1.txt");

            Console.WriteLine("Hello World!");
        }
    }
}
