using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.HttpServer.RequestHandlers.Base;

public interface IHttpRequestHandler
{
    public void SetNextHandler(IHttpRequestHandler requestHandler);
    public void RequestHandle(HttpListenerContext context);
}
