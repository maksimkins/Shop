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

public class ProductPostHandler : IRequestHandler
{
    private readonly ProductEFCoreRepository productRepository;

    public ProductPostHandler()
    {
        productRepository = new ProductEFCoreRepository();
    }

    public async void RequestHandle(HttpListenerContext context)
    {
        await RequestPostProduct(context);
    }

    private async Task RequestPostProduct(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";
            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            Product product = JsonSerializer.Deserialize<Product>(body)
                ?? throw new ArgumentNullException("body of product request is corrupted");

            productRepository.Post(product);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 201;
            await writer.WriteLineAsync("product posted succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't post product) {ex.Message}");
        }
    }
}
