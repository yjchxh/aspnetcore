// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication.AzureAD.UI;

[Obsolete("This is obsolete and will be removed in a future version. Use Microsoft.Identity.Web instead. See https://aka.ms/ms-identity-web.")]
internal class AzureADOpenIdConnectOptionsConfiguration : IConfigureNamedOptions<OpenIdConnectOptions>
{
    private readonly IOptions<AzureADSchemeOptions> _schemeOptions;
    private readonly IOptionsMonitor<AzureADOptions> _azureADOptions;

    public AzureADOpenIdConnectOptionsConfiguration(IOptions<AzureADSchemeOptions> schemeOptions, IOptionsMonitor<AzureADOptions> azureADOptions)
    {
        _schemeOptions = schemeOptions;
        _azureADOptions = azureADOptions;
    }

    public void Configure(string name, OpenIdConnectOptions options)
    {
        var azureADScheme = GetAzureADScheme(name);
        if (azureADScheme is null)
        {
            return;
        }

        var azureADOptions = _azureADOptions.Get(azureADScheme);
        if (name != azureADOptions.OpenIdConnectSchemeName)
        {
            return;
        }

        options.ClientId = azureADOptions.ClientId;
        options.ClientSecret = azureADOptions.ClientSecret;
        options.Authority = new Uri(new Uri(azureADOptions.Instance), azureADOptions.TenantId).ToString();
        options.CallbackPath = azureADOptions.CallbackPath ?? options.CallbackPath;
        options.SignedOutCallbackPath = azureADOptions.SignedOutCallbackPath ?? options.SignedOutCallbackPath;
        options.SignInScheme = azureADOptions.CookieSchemeName;
        options.UseTokenLifetime = true;
    }

    private string GetAzureADScheme(string name)
    {
        foreach (var mapping in _schemeOptions.Value.OpenIDMappings)
        {
            if (mapping.Value.OpenIdConnectScheme == name)
            {
                return mapping.Key;
            }
        }

        return null;
    }

    public void Configure(OpenIdConnectOptions options)
    {
    }
}
