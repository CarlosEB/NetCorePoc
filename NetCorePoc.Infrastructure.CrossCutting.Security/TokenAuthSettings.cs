namespace NetCorePoc.Infrastructure.CrossCutting.Security
{
    public class TokenAuthSettings
    {
        public string Key { get; set; }
        public string SiteUrl { get; set; }
        public string TokenCreationPath { get; set; }
    }
}
