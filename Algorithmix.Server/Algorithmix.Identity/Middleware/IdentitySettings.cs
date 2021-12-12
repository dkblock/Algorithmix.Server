namespace Algorithmix.Identity.Middleware
{
    public class IdentitySettings
    {
        public string Secret { get; set; }
        public int AccessTokenLifetimeInMinutes { get; set; }
        public int RefreshTokenLifetimeInDays { get; set; }
    }
}
