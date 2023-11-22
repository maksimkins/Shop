using Microsoft.EntityFrameworkCore;
using ServerApp.Repositories.EF_Core;
using SharedProj;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp.HttpServer;

public class HttpServer  
{
    private readonly HttpListener listener;
    private readonly ProductEFCoreRepositories repository;


    public HttpServer(int port)
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://*:{port}/");
        listener.Start();

        repository = new ProductEFCoreRepositories();
        Console.WriteLine($"server started on port: {port}");
    }

    public async void HttpListen()
    {
        
        
        HttpListenerContext context = await listener.GetContextAsync();

        if (context is null)
        {
            return;
        }

        RequestHandle(context);
        
    }

    private async void RequestHandle(HttpListenerContext context)
    {
        Console.WriteLine($"{context.Request.UserHostName} connected to server");

        string[]? urlItems = context.Request.RawUrl?.Split('/');

        if(urlItems is null)
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"The requested resource was not found");
            return;
        }


        if (urlItems.Contains("Product"))
            await RequestHandleProduct(context, urlItems);

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"The requested resource was not found");
            return;
        }

    }

    private async Task RequestHandleProduct(HttpListenerContext context, string[] urlItems)// POST, GET, PUT, DELETE
    {
        int id = -1;
        bool HasId = false;


        string? item = urlItems.LastOrDefault();

        if (item is null || int.TryParse(item, out id))
        {
            HasId = true;
        }
        

        if (HasId && context.Request.HttpMethod == "GET")
            await RequestGetProduct(context, id);

        else if (HasId && context.Request.HttpMethod == "DELETE")
            await RequestDeleteProduct(context, id);

        else if (HasId && context.Request.HttpMethod == "PUT")
            await RequestUpdateProduct(context, id);

        else if (context.Request.HttpMethod == "GET")
            await RequestGetAllProducts(context);

        else if (context.Request.HttpMethod == "POST")
            await RequestPostProduct(context);

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync("The requested resource was not found");

            return;
        }

    }

    private async Task RequestPostProduct(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";
            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            Product product = JsonSerializer.Deserialize<Product>(body)
                ?? throw new ArgumentNullException("body is corrupted");

            repository.Post(product);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 201;
            await writer.WriteLineAsync("Posted succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't change product) {ex.Message}");
        }
    }

    private async Task RequestGetAllProducts(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";

            IEnumerable<Product> products = repository.GetAll();

            string prods = JsonSerializer.Serialize(products);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync(prods);

        }
        catch (Exception ex)
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"The requested resource was not found {ex.Message}");
        }
    }
    private async Task RequestUpdateProduct(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            Product product = JsonSerializer.Deserialize<Product>(body) 
                ?? throw new ArgumentNullException("body is corrupted");

            repository.Update(id, product);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync("Updated succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't change product) {ex.Message}");
        }
    }

    private async Task RequestGetProduct(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            Product product = repository.GetById(id);
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

    private async Task RequestDeleteProduct(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            repository.Delete(id);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync("Deleted succesfully");
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
