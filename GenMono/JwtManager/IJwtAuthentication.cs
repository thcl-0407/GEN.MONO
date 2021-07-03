namespace JwtManager
{
    public interface IJwtAuthentication
    {
        string GenerateAccessToken(TokenIdentity identity);

        bool isValidToken(string CurrentToken);

        string GetValue(GetClaimType getClaimType, string CurrentToken);
    }
}
