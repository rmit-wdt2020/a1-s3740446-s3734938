using System;
using System.Threading.Tasks;

namespace BankingApplication
{
    class MainClass
    {

        public static async Task Main(string[] args)
        {
            await DatabaseAccess.Instance.GetJson();
            //Driver driver = new Driver();
            //driver.performLogin();
        }

    }
}
