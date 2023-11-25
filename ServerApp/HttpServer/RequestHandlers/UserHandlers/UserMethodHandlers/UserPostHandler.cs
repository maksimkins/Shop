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

public class UserPostHandler : IRequestHandler
{
    private readonly UserLogic userLogic;
    public UserPostHandler()
    {
        userLogic = new UserLogic(new UserEFCoreRepository());
    }

    public async void RequestHandle(HttpListenerContext context)
    {
        string[]? urlItems = context.Request.RawUrl?.Split('/');

        if (urlItems is not null && urlItems.Contains("Authentication"))
        {
            await RequestPostGetUser(context);
        }
        else
        {
            await RequestPostUser(context);
        }
    }

    private async Task RequestPostUser(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";

            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            User user = JsonSerializer.Deserialize<User>(body)
                ?? throw new ArgumentNullException("body of user request is corrupted");

            userLogic.Post(user);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 201;
            await writer.WriteLineAsync("user posted succesfully");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't post user) {ex.Message}");
        }
    }

    private async Task RequestPostGetUser(HttpListenerContext context)
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
