namespace AskIt.Core.Configuration
{
    public class AuthConfiguration
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string IssuerSigningKey { get; set; }
    }
}
