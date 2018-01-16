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

            Console.WriteLine("Hello World!");
        }
    }
}
