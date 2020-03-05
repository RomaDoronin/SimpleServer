using SimpleHTTPServer.HTTPInteraction;

namespace SimpleHTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server(80);
        }
    }
}
