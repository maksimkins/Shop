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

namespace ServerApp.HttpServer.RequestHandlers.UserHandlers.UserMethodHandlers;

public class UserPostHandler : IRequestHandler
{
    private readonly UserEFCoreRepository userRepository;
    public UserPostHandler()
    {
        userRepository = new UserEFCoreRepository();
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
            await RequestPostUser(context);
        }

        else
        {
            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync("Wrong endpoint");

            return;
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

            userRepository.Post(user);

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
}
