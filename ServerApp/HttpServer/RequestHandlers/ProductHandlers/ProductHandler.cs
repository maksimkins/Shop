using ServerApp.HttpServer.RequestHandlers.Base;
using ServerApp.HttpServer.RequestHandlers.ProductHandlers.ProductMethodHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.HttpServer.RequestHandlers.ProductHandlers;

public class ProductHandler : IRequestHandler
{
    private IRequestHandler? productGetHandler;
    private IRequestHandler? productPutHandler;
    private IRequestHandler? productPostHandler;
    private IRequestHandler? productDeleteHandler;
    public async void RequestHandle(HttpListenerContext context)
    {
        {
            string[]? urlItems = context.Request.RawUrl?.Split('/');

            string? item = urlItems?.LastOrDefault();

            if (context.Request.HttpMethod == HttpMethod.Get.Method)
            {
                productGetHandler = new ProductGetHandler();
                productGetHandler.RequestHandle(context);
            }


            else if (context.Request.HttpMethod == HttpMethod.Delete.Method)
            {
                productDeleteHandler = new ProductDeleteHandler();
                productDeleteHandler.RequestHandle(context);
            }


            else if (context.Request.HttpMethod == HttpMethod.Put.Method)
            {
                productPutHandler = new ProductPutHandler();
                productPutHandler.RequestHandle(context);
            }
           

            else if (context.Request.HttpMethod == HttpMethod.Post.Method)
            {
                productPostHandler = new ProductPostHandler();
                productPostHandler.RequestHandle(context);
            }
                

            else
            {
                using var writer = new StreamWriter(context.Response.OutputStream);
                context.Response.StatusCode = 404;
                await writer.WriteLineAsync("Wrong endpoint");

                return;
            }

        }
    }
}
