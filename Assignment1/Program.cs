using System;
using System.Threading.Tasks;

namespace BankingApplication
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            //Check for populated database
            if (DatabaseAccess.Instance.DbChk("dbo.CheckDB") == 1)
            {
                await DatabaseAccess.Instance.GetJson();
            }
            Driver driver = new Driver();
            driver.performLogin();
        }

    }
}
