using Biblioteket.LibraryClient.Core;

namespace Biblioteket.LibraryClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new Startup();
            app.Run();
        }
    }
}