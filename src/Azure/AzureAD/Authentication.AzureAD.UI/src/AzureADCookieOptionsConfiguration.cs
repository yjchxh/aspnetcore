// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication.AzureAD.UI;

[Obsolete("This is obsolete and will be removed in a future version. Use Microsoft.Identity.Web instead. See https://aka.ms/ms-identity-web.")]
internal class AzureADCookieOptionsConfiguration : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    private readonly IOptions<AzureADSchemeOptions> _schemeOptions;
    private readonly IOptionsMonitor<AzureADOptions> _AzureADOptions;

    public AzureADCookieOptionsConfiguration(IOptions<AzureADSchemeOptions> schemeOptions, IOptionsMonitor<AzureADOptions> AzureADOptions)
    {
        _schemeOptions = schemeOptions;
        _AzureADOptions = AzureADOptions;
    }

    public void Configure(string name, CookieAuthenticationOptions options)
    {
        var AzureADScheme = GetAzureADScheme(name);
        if (AzureADScheme is null)
        {
            return;
        }

        var AzureADOptions = _AzureADOptions.Get(AzureADScheme);
        if (name != AzureADOptions.CookieSchemeName)
        {
            return;
        }

        options.LoginPath = $"/AzureAD/Account/SignIn/{AzureADScheme}";
        options.LogoutPath = $"/AzureAD/Account/SignOut/{AzureADScheme}";
        options.AccessDeniedPath = "/AzureAD/Account/AccessDenied";
        options.Cookie.SameSite = SameSiteMode.None;
    }

    public void Configure(CookieAuthenticationOptions options)
    {
    }

    private string GetAzureADScheme(string name)
    {
        foreach (var mapping in _schemeOptions.Value.OpenIDMappings)
        {
            if (mapping.Value.CookieScheme == name)
            {
                return mapping.Key;
            }
        }

        return null;
    }
}
