using ServerApp.HttpServer.RequestHandlers.Base;
using ServerApp.Repositories.EF_Core;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp.HttpServer.RequestHandlers.ProductHandlers;

public class ProductPutHandler : IRequestHandler
{
    private readonly ProductEFCoreRepository productRepository;
    public ProductPutHandler()
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
            await RequestPutProduct(context, id);
        }

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync("Wrong endpoint");

            return;
        }

    }
    private async Task RequestPutProduct(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            Product product = JsonSerializer.Deserialize<Product>(body)
                ?? throw new ArgumentNullException("body of product request is corrupted");

            productRepository.Update(id, product);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync("product updated succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't change product) {ex.Message}");
        }
    }
}
