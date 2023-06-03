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

    public static async Task<string> SendRequestAsync(string url, HttpMethod method, string token = null)
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync("ERROR: " + ex.Message + "\n\n" + ex.StackTrace);
            return null;
        }
    }

    //request que me devolve um JSON com um objeto generico serializado!
    public static async Task<TResponse> SendRequestAsyncObject<TResponse>(string url, HttpMethod method, string token = null)
    {
        HttpRequestMessage request = new HttpRequestMessage(method, url);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        TResponse responseObject = JsonSerializer.Deserialize<TResponse>(responseBody);

        return responseObject;
    }
    
}
