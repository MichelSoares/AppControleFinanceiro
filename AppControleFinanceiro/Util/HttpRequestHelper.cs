using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Util;

public class HttpRequestHelper
{
    private static HttpClient _httpClient;

    static HttpRequestHelper()
    {
        _httpClient = new HttpClient();
        //_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("APP-CONTROLE");//posso futuramente somente aceitar requisições com um UserAgente específico
    }

    public static async Task<string> SendRequestAsync(string url, HttpMethod method, string token = null)
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
}
