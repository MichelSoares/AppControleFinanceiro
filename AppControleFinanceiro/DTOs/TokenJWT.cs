using System.Text.Json.Serialization;

namespace AppControleFinanceiro.DTOs;

public class TokenJWT
{
    [JsonPropertyName("authenticated")]
    public bool Authenticated { get; set; }

    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
