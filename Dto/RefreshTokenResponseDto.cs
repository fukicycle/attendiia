using System.Text.Json.Serialization;

namespace Attendiia.Dto;

public sealed class RefreshTokenResponseDto
{
    public RefreshTokenResponseDto(string accessToken, int expiresIn, string tokenType, string refreshToken, string idToken, string userId, string projectId)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        TokenType = tokenType;
        RefreshToken = refreshToken;
        IdToken = idToken;
        UserId = userId;
        ProjectId = projectId;
    }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; }
    [JsonPropertyName("id_token")]
    public string IdToken { get; }
    [JsonPropertyName("user_id")]
    public string UserId { get; }
    [JsonPropertyName("project_id")]
    public string ProjectId { get; }
}
