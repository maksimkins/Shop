using ServerApp.HttpServer.RequestHandlers.Base;
using ServerApp.HttpServer.RequestHandlers.ProductHandlers.ProductMethodHandlers;
using ServerApp.HttpServer.RequestHandlers.UserHandlers.UserMethodHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.HttpServer.RequestHandlers.UserHandlers;

public class UserHandler : IRequestHandler
{
    private IRequestHandler? userGetHandler;
    private IRequestHandler? userPostHandler;

    public async void RequestHandle(HttpListenerContext context)
    {

        string[]? urlItems = context.Request.RawUrl?.Split('/');

        string? item = urlItems?.LastOrDefault();

        if (context.Request.HttpMethod == HttpMethod.Get.Method)
        {
            userGetHandler = new UserGetHandler();
            userGetHandler.RequestHandle(context);
        }

        else if (context.Request.HttpMethod == HttpMethod.Post.Method)
        {
            userPostHandler = new UserPostHandler();
            userPostHandler.RequestHandle(context);
        }


        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync("Wrong endpoint (UserHandler)");

            return;
        }
    }
}
