using ServerApp.HttpServer.RequestHandlers.Base;
using ServerApp.Repositories.Base;
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

public class ProductGetHandler : IRequestHandler
{
    private readonly ProductEFCoreRepository productRepository;

    public ProductGetHandler()
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
            await RequestGetProduct(context, id);
        }

        else
        {
            await RequestGetAllProducts(context);
        }
    }

    private async Task RequestGetAllProducts(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";

            IEnumerable<Product> products = productRepository.GetAll();

            string prods = JsonSerializer.Serialize(products);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync(prods);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"error in getting all products{ex.Message}");
        }
    }

    private async Task RequestGetProduct(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            Product product = productRepository.GetById(id);
            string prod = JsonSerializer.Serialize(product);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync(prod);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't find product) {ex.Message}");
        }
    }

}
