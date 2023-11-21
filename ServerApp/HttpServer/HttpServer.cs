using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.HttpServer;

public class HttpServer  
{
    private readonly HttpListener listener;

    public HttpServer(int port)
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://*:{port}/");
        listener.Start();
    }

    public async void HttpListen()
    {
        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();

            if (context is null)
            {
                break;
            }

            RequestHandle(context);
        }
    }

    public async void RequestHandle(HttpListenerContext context)// TO DO
                                                                // create logic that depends on Request (will check reques method) and call specific method
    {

        Console.WriteLine($"{context.User} connected to server");

        await ResponseSender(context);
    }

    public async Task ResponseSender(HttpListenerContext context)// instance of specific method
    {
        using var writer = new StreamWriter(context.Response.OutputStream);
        context.Response.StatusCode = 200;
        await writer.WriteLineAsync("you succesfully conected to the server");
    }

}
