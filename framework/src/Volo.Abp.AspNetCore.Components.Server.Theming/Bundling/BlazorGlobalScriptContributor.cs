﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;

public class BlazorGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreComponentsWebOptions>>().Value;
        context.Files.AddIfNotContains(options.IsBlazorWebApp
            ? "/_framework/blazor.web.js"
            : "/_framework/blazor.server.js");
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");
    }
}
