using ServerApp.HttpServer.RequestHandlers.Base;
using ServerApp.Repositories.Base;
using ServerApp.Repositories.EF_Core;
using ServerApp.Repositories.Logic_Classes;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp.HttpServer.RequestHandlers.ProductHandlers.ProductMethodHandlers;

public class ProductGetHandler : IRequestHandler
{
    private readonly ProductLogic productLogic;

    public ProductGetHandler()
    {
        productLogic = new ProductLogic(new ProductEFCoreRepository());
    }
    public async void RequestHandle(HttpListenerContext context)
    {
        int id = -1;
        bool HasId = false;
        string[]? urlItems = context.Request.RawUrl?.Split('/');

        string? title = context.Request.QueryString["title"];
        string? text = context.Request.QueryString["text"];
        string? pricefromstr = context.Request.QueryString["pricefrom"];
        string? pricetostr = context.Request.QueryString["priceto"];

        string? useridstr = context.Request.QueryString["user_id"];

        ProductDTO filter = new ProductDTO()
        {
            Title = title,
            Text = text,
            PriceFrom = double.TryParse(pricefromstr, out double pricefrom) ? pricefrom : null,
            PriceTo = double.TryParse(pricetostr, out double priceto) ? priceto : null,
        };

        string? item = urlItems?.LastOrDefault();

        if (item is null || int.TryParse(item, out id))
        {
            HasId = true;
        }

        if (HasId)
        {
            await RequestGetProduct(context, id);
        }

        else if(context.Request.QueryString.Count > 0)
        {
            await RequestGetAllFilteredProducts(context, filter);
        }

        else if (int.TryParse(useridstr, out int userid))
        {
            await RequestGetAllByUserId(context, userid);
        }

        else
        {
            await RequestGetAllProducts(context);
        }

    }

    private async Task RequestGetAllByUserId(HttpListenerContext context, int userid)
    {
        try
        {
            IEnumerable<Product> products = productLogic.GetAllByUserId(userid);

            string prods = JsonSerializer.Serialize(products);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync(prods);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"error in getting all user {userid} products{ex.Message}");
        }
    }

    private async Task RequestGetAllFilteredProducts(HttpListenerContext context, ProductDTO filter)
    {
        try
        {
            IEnumerable<Product> products = productLogic.Filter(filter);

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
            await writer.WriteLineAsync($"error in getting all filtered products{ex.Message}");
        }
        
    }

    private async Task RequestGetAllProducts(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";

            IEnumerable<Product> products = productLogic.GetAll();

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

            Product product = productLogic.GetById(id);
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
