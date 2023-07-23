namespace FindMyTeddy.API
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public List<string> ReactAppURL { get; set; }
        public AuthSettings AuthSettings { get; set; }
        public string AppName { get; set; }
    }
    public class AuthSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string RefreshTokenName { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int AccessTokenExpiryInMinutes { get; set; }
        public string JwtSecret { get; set; }
    }

    public class ConnectionStrings
    {
        public string FindMyTeddyConnection { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }

    
}
