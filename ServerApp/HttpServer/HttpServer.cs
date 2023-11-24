using Microsoft.EntityFrameworkCore;
using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core;
using SharedProj;
using SharedProj.Models;
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

using RequestHandlers.Base;
using ServerApp.HttpServer.RequestHandlers.ProductHandlers;
using ServerApp.HttpServer.RequestHandlers.UserHandlers;

public class HttpServer  
{
    private readonly HttpListener listener;

    //private readonly ProductEFCoreRepository productRepository;
    //private readonly UserEFCoreRepository userRepository;

    private IRequestHandler? productHandler;
    private IRequestHandler? userHandler;


    public HttpServer(int port)
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://*:{port}/");
        listener.Start();

        //productRepository = new ProductEFCoreRepository();
        //userRepository = new UserEFCoreRepository();

        userHandler = new UserHandler();
        productHandler = new ProductHandler();

        Console.WriteLine($"server started on port: {port}");
    }

    public void HttpListen()
    {
        HttpListenerContext context = listener.GetContext();

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
            await writer.WriteLineAsync($"The requested resource was not found (HttpServer)");
            return;
        }


        if (urlItems.Contains("Product"))
        {
            productHandler?.RequestHandle(context);
        }
            
        else if (urlItems.Contains("User"))
        {
            userHandler?.RequestHandle(context);
        }
        

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"endpoint doesn't Product or User");
            return;
        }

    }

    //#region Request handlers for products
    ////private async Task RequestHandleProduct(HttpListenerContext context, string[] urlItems)// POST, GET, PUT, DELETE
    ////{
    ////    int id = -1;
    ////    bool HasId = false;


    ////    string? item = urlItems.LastOrDefault();

    ////    if (item is null || int.TryParse(item, out id))
    ////    {
    ////        HasId = true;
    ////    }
        

    ////    if (HasId && context.Request.HttpMethod == HttpMethod.Get.Method)
    ////        await RequestGetProduct(context, id);

    ////    else if (HasId && context.Request.HttpMethod == HttpMethod.Delete.Method)
    ////        await RequestDeleteProduct(context, id);

    ////    else if (HasId && context.Request.HttpMethod == HttpMethod.Put.Method)
    ////        await RequestPutProduct(context, id);

    ////    else if (context.Request.HttpMethod == HttpMethod.Get.Method)
    ////        await RequestGetAllProducts(context);

    ////    else if (context.Request.HttpMethod == HttpMethod.Post.Method)
    ////        await RequestPostProduct(context);

    ////    else
    ////    {
    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 404;
    ////        await writer.WriteLineAsync("Wrong endpoint");

    ////        return;
    ////    }

    ////}

    ////private async Task RequestPostProduct(HttpListenerContext context)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType = "application/json";
    ////        using var bodyStream = new StreamReader(context.Request.InputStream);
    ////        string body = bodyStream.ReadToEnd();

    ////        Product product = JsonSerializer.Deserialize<Product>(body)
    ////            ?? throw new ArgumentNullException("body of product request is corrupted");

    ////        productRepository.Post(product);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 201;
    ////        await writer.WriteLineAsync("product posted succesfully");
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 400;
    ////        await writer.WriteLineAsync($"Bad Request (couldn't post product) {ex.Message}");
    ////    }
    ////}

    ////private async Task RequestGetAllProducts(HttpListenerContext context)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType = "application/json";

    ////        IEnumerable<Product> products = productRepository.GetAll();

    ////        string prods = JsonSerializer.Serialize(products);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 200;
    ////        await writer.WriteLineAsync(prods);

    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 404;
    ////        await writer.WriteLineAsync($"error in getting all products{ex.Message}");
    ////    }
    ////}
    ////private async Task RequestPutProduct(HttpListenerContext context, int id)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType = "application/json";

    ////        using var bodyStream = new StreamReader(context.Request.InputStream);
    ////        string body = bodyStream.ReadToEnd();

    ////        Product product = JsonSerializer.Deserialize<Product>(body) 
    ////            ?? throw new ArgumentNullException("body of product request is corrupted");

    ////        productRepository.Update(id, product);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 200;
    ////        await writer.WriteLineAsync("product updated succesfully");
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 400;
    ////        await writer.WriteLineAsync($"Bad Request (couldn't change product) {ex.Message}");
    ////    }
    ////}

    ////private async Task RequestGetProduct(HttpListenerContext context, int id)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType = "application/json";

    ////        Product product = productRepository.GetById(id);
    ////        string prod = JsonSerializer.Serialize(product);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 200;
    ////        await writer.WriteLineAsync(prod);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 400;
    ////        await writer.WriteLineAsync($"Bad Request (couldn't find product) {ex.Message}");
    ////    }    
    ////}

    ////private async Task RequestDeleteProduct(HttpListenerContext context, int id)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType = "application/json";

    ////        productRepository.Delete(id);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 200;
    ////        await writer.WriteLineAsync("product deleted succesfully");
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 400;
    ////        await writer.WriteLineAsync($"Bad Request (couldn't delete product) {ex.Message}");
    ////    }
    ////}
    //#endregion

    //#region Request handlers for users

    ////private async Task RequestHandleUser(HttpListenerContext context, string[] urlItems)
    ////{
    ////    if (context.Request.HttpMethod == HttpMethod.Post.Method)
    ////        await RequestPostUser(context);
    ////    else if(context.Request.HttpMethod == HttpMethod.Get.Method)
    ////        await RequestGetUser(context);
    ////    else
    ////    {
    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 404;
    ////        await writer.WriteLineAsync("Wrong endpoint");

    ////        return;
    ////    }
    ////}

    ////private async Task RequestPostUser(HttpListenerContext context)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType= "application/json";

    ////        using var bodyStream = new StreamReader(context.Request.InputStream);
    ////        string body = bodyStream.ReadToEnd();

    ////        User user = JsonSerializer.Deserialize<User>(body)
    ////            ?? throw new ArgumentNullException("body of user request is corrupted");

    ////        userRepository.Post(user);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 201;
    ////        await writer.WriteLineAsync("user posted succesfully");

    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 400;
    ////        await writer.WriteLineAsync($"Bad Request (couldn't post user) {ex.Message}");
    ////    }
    ////}

    ////private async Task RequestGetUser(HttpListenerContext context)
    ////{
    ////    try
    ////    {
    ////        context.Response.ContentType = "application/json";

    ////        using var bodyStream = new StreamReader(context.Request.InputStream);
    ////        string body = bodyStream.ReadToEnd();

    ////        User user = JsonSerializer.Deserialize<User>(body)
    ////            ?? throw new ArgumentNullException("body of user request is corrupted");

    ////        bool isRegistered = userRepository.IsRegistered(user);
    ////        string isregistered = JsonSerializer.Serialize(isRegistered);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 200;
    ////        await writer.WriteLineAsync(isregistered);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        Console.WriteLine(ex.Message);

    ////        using var writer = new StreamWriter(context.Response.OutputStream);
    ////        context.Response.StatusCode = 400;
    ////        await writer.WriteLineAsync($"Bad Request (couldn't find user) {ex.Message}");
    ////    }
    ////}

    //#endregion
}
