using System.Threading.Tasks;

namespace BankingApplication
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            //Check for seeded database
            if (DatabaseAccess.Instance.DbChk("dbo.CheckDB") == 1)
            {
                //If not seeded, get web services and seed database
                await DatabaseAccess.Instance.GetJson();
            }
            Driver driver = new Driver();
            driver.PerformLogin();
        }

    }
}
