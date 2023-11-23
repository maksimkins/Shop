using ServerApp.HttpServer.RequestHandlers.Base;
using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.HttpServer.RequestHandlers.ProductHandlers;

public class ProductDeleteHandler : IRequestHandler
{
    private readonly ProductEFCoreRepository productRepository;
    public ProductDeleteHandler()
    {
        productRepository = new ProductEFCoreRepository();
    }
    public async void RequestHandle(HttpListenerContext context)
    {
        int id = -1;
        bool HasId = false;
        string[]? urlItems = context.Request.RawUrl?.Split('/');

        string? item = urlItems?.LastOrDefault();

        if (item is null || int.TryParse(item, out id))
        {
            HasId = true;
        }

        if (HasId)
        {
            await RequestDeleteProduct(context, id);
        }

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync("Wrong endpoint");

            return;
        }
    }

    private async Task RequestDeleteProduct(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            productRepository.Delete(id);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync("product deleted succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't delete product) {ex.Message}");
        }
    }
}
