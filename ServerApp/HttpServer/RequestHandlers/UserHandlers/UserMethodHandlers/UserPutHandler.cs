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

public class UserPutHandler : IRequestHandler
{
    private readonly UserLogic userLogic;
    public UserPutHandler()
    {
        userLogic = new UserLogic(new UserEFCoreRepository());
    }

    public async void RequestHandle(HttpListenerContext context)
    {
        try
        {
            await RequestPutUser(context);
        }
        catch(Exception ex)
        {

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 404;
            await writer.WriteLineAsync($"Wrong endpoint {ex.Message}");

        }    
    }
    private async Task RequestPutUser(HttpListenerContext context)
    {
        try
        {
            context.Response.ContentType = "application/json";

            using var bodyStream = new StreamReader(context.Request.InputStream);
            string body = bodyStream.ReadToEnd();

            User user = JsonSerializer.Deserialize<User>(body)
                ?? throw new ArgumentNullException("body of user request is corrupted");

            userLogic.Update(user);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 200;
            await writer.WriteLineAsync("user updated succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            using var writer = new StreamWriter(context.Response.OutputStream);
            context.Response.StatusCode = 400;
            await writer.WriteLineAsync($"Bad Request (couldn't change user) {ex.Message}");
        }
    }
}
