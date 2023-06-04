using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace AppControleFinanceiro.Util;

public class HttpRequestHelper
{
    private static readonly HttpClient _httpClient = new HttpClient();

    static HttpRequestHelper()
    {
        _httpClient = new HttpClient();
        //_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("APP-CONTROLE"); //posso futuramente somente aceitar requisições com um UserAgente específico
    }

    public static async Task<string> SendRequestAsync(string endpoint, HttpMethod method, string token = null)
    {
        
        HttpRequestMessage request = new HttpRequestMessage(method, endpoint);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
        
    }

    //request que me devolve um JSON com um objeto genérico deserializado, resposta tbm genérica.
    public static async Task<TResponse> SendRequestAsyncObject<TResponse>(string endpoint, HttpMethod method, string token = null, object obj = null)
    {

        HttpRequestMessage request = new HttpRequestMessage(method, endpoint);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (obj != null)
        {
            var json = JsonSerializer.Serialize(obj);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responsePrimitivo = await response.Content.ReadAsStringAsync();

        if (typeof(TResponse) == typeof(string))
        {
            return (TResponse)Convert.ChangeType(responsePrimitivo, typeof(TResponse));
        }

        TResponse responseObject = JsonSerializer.Deserialize<TResponse>(responsePrimitivo);
        return responseObject;
    }
    
}
