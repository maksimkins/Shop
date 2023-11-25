using ServerApp.HttpServer.RequestHandlers.Base;
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

namespace ServerApp.HttpServer.RequestHandlers.UserHandlers.UserMethodHandlers;

public class UserGetHandler : IRequestHandler
{
    private readonly UserLogic userLogic;
    public UserGetHandler()
    {
        userLogic = new UserLogic(new UserEFCoreRepository());
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

        if (!HasId)
        {
            await RequestGetUser(context);
        }

        else if (HasId)
        {
            await RequestGetByIdUser(context, id);
        }

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync("Wrong endpoint");

            return;
        }
    }

    private async Task RequestGetByIdUser(HttpListenerContext context, int id)
    {
        try
        {
            context.Response.ContentType = "application/json";

            User user = userLogic.GetById(id);
            string userstr = JsonSerializer.Serialize(user);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync(userstr);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't find product) {ex.Message}");
        }
    }
    private async Task RequestGetUser(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";

            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            User user = JsonSerializer.Deserialize<User>(body)
                ?? throw new ArgumentNullException("body of user request is corrupted");

            User registeredUser = userLogic.IsRegistered(user);
            string isregistered = JsonSerializer.Serialize(registeredUser);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync(isregistered);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't find user) {ex.Message}");
        }
    }
}
