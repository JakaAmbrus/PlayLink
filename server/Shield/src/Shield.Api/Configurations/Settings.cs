namespace Shield.Api.Configurations;

public class Settings
{
    public AzureConfiguration AzureConfiguration { get; set; }
}

public class AzureConfiguration
{
    public string AzureAdInstance { get; set; }
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string CallbackPath { get; set; }
}