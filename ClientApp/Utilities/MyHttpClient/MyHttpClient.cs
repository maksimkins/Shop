using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.Utilities.MyHttpClient;

public class MyHttpClient
{
    private readonly HttpClient _httpClient;

    public MyHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public static StringContent ConvertObject<T>(T obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    public Task<HttpResponseMessage> GetAsync(string uri)
    {
       return _httpClient.GetAsync(uri);
    }

    public Task<HttpResponseMessage> PostAsync<T>(string uri, T obj) {
        var content = ConvertObject(obj);
        return _httpClient.PostAsync(uri, content);
    }

    public Task<HttpResponseMessage> PutAsync<T>(string uri, T obj)
    {
        var content = ConvertObject(obj);
        return _httpClient.PutAsync(uri, content);
    }
}
