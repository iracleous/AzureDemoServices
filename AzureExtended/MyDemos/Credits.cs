using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AzureExtended.MyDemos;


// WIP


internal static class Credits
{

    //public static async Task Main(string[] args)
    //{


    //}

    /// <summary>
    /// Creates an MSAL Confidential client application
    /// </summary>
    /// <param name="httpContext">HttpContext associated with the OIDC response</param>
    /// <param name="claimsPrincipal">Identity for the signed-in user</param>
    /// <returns></returns>
    //private IConfidentialClientApplication BuildConfidentialClientApplication(HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
    //{
    //    var request = httpContext.Request;

    //    // Find the URI of the application)
    //    string currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, azureAdOptions.CallbackPath ?? string.Empty);

    //    // Updates the authority from the instance (including national clouds) and the tenant
    //    string authority = $"{azureAdOptions.Instance}{azureAdOptions.TenantId}/";

    //    // Instantiates the application based on the application options (including the client secret)
    //    var app = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(_applicationOptions)
    //                  .WithRedirectUri(currentUri)
    //                  .WithAuthority(authority)
    //                  .Build();

    //    // Initialize token cache providers. In the case of Web applications, there must be one
    //    // token cache per user. 
    //    // Here the key of the token cache is in the claimsPrincipal
    //    // which contains the identity of the signed-in user,
    //    if (this.UserTokenCacheProvider != null)
    //    {
    //        this.UserTokenCacheProvider.Initialize(app.UserTokenCache, httpContext, claimsPrincipal);
    //    }
    //    if (this.AppTokenCacheProvider != null)
    //    {
    //        this.AppTokenCacheProvider.Initialize(app.AppTokenCache, httpContext);
    //    }
    //    return app;
    //}




}