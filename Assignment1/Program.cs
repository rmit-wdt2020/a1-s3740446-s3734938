using System;

namespace HelloWorldApplication
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            DatabaseAccess.establishConnection();
            Driver driver = new Driver();
            driver.performLogin();
        }

    }
}
