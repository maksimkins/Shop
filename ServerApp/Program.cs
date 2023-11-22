namespace ServerApp;

using ServerApp.HttpServer;

class Program
{
    static void Main(string[] args)
    {
        HttpServer.HttpServer httpServer = new HttpServer.HttpServer(8080);

        while (true)
        {
            httpServer.HttpListen();
        }
    }
}
