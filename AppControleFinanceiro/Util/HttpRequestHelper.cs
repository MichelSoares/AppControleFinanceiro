using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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
    public static async Task<TResponse> SendRequestAsyncObject<TResponse>(string endpoint, HttpMethod method, string token = null, object obj = null, string parametroUriKey = null, string parametroUriValue = null)
    {
        HttpRequestMessage request;

        //QueryParameter
        if(parametroUriKey is not null && parametroUriValue is not null)
        {
            UriBuilder uriBuilder = new UriBuilder(endpoint);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            queryParams[parametroUriKey] = parametroUriValue;
            uriBuilder.Query = queryParams.ToString();
            request = new HttpRequestMessage(method, uriBuilder.Uri);
        } 
        else if (parametroUriValue is not null && parametroUriKey is null)
        {
            request = new HttpRequestMessage(method, endpoint + "/" + parametroUriValue);
        } 
        else
        {
            //request = parametroUriValue == null ? new HttpRequestMessage(method, endpoint) : new HttpRequestMessage(method, endpoint + "/" + parametroUriValue);
            request = new HttpRequestMessage(method, endpoint);
        }

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
