using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Biblioteket.LibraryClient.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void Run()
        {
            AdminUser? currentUser = null;

            while (currentUser == null)
                LoginMenu.Show(ref currentUser);

            MainMenu.ShowMainMenu(currentUser);
        }
    }
}
