using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Biblioteket.LibraryClient.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "LibraryClient", "appsettings.json");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(path, optional: false, reloadOnChange: true)
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
