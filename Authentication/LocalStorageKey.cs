namespace Attendiia.Authentication;

public static class LocalStorageKey
{
    private const string APP_ID = "b0b9b893-0644-4c55-b8d3-ab66fb2c9a0d";
    public const string ID_TOKEN = $"{APP_ID}_id_token";
    public const string REFRESH_TOKEN = $"{APP_ID}_refresh_token";
    public const string USER_INFO = $"{APP_ID}_user_info";
}