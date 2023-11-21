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

   

}
