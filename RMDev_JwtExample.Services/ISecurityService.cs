namespace RMDev_JwtExample.Services
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
    }
}