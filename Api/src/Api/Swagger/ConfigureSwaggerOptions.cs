using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IConfiguration _config;

    public ConfigureSwaggerOptions(IConfiguration config)
    {
        _config = config;
    }

    public void Configure(SwaggerGenOptions options)
    {
        var disco = GetDiscoveryDocument();

        var apiScope = _config.GetValue<string>("Authentication:ApiName");
        var scopes = apiScope.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        var addScopes = _config.GetValue<string>("Authentication:AdditionalScopes");
        var additionalScopes = addScopes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        scopes.AddRange(additionalScopes);

        var oauthScopeDic = new Dictionary<string, string>();
        foreach (var scope in scopes)
        {
            oauthScopeDic.Add(scope, scope);
        }

        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Preferences API Documentation",
            Version = "v1.0",
            Description = ""
        });

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
                    TokenUrl = new Uri(disco.TokenEndpoint),
                    Scopes = oauthScopeDic
                }
            }
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                },
                oauthScopeDic.Keys.ToArray()
            }
        });
    }

    private DiscoveryDocumentResponse GetDiscoveryDocument()
    {
        var client = new HttpClient();
        var authority = _config.GetValue<string>("Authentication:Authority");
        return client.GetDiscoveryDocumentAsync(authority)
            .GetAwaiter()
            .GetResult();
    }
}
