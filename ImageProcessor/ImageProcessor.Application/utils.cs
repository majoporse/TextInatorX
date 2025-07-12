using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImageProcessor.Application;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal class DevOnlyAttribute : Attribute, IFilterFactory
{
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new DevOnlyImplAttribute(serviceProvider.GetRequiredService<IWebHostEnvironment>());
    }

    public bool IsReusable => true;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    private sealed class DevOnlyImplAttribute : Attribute, IAuthorizationFilter
    {
        public DevOnlyImplAttribute(IWebHostEnvironment hostingEnv)
        {
            HostingEnv = hostingEnv;
        }

        private IWebHostEnvironment HostingEnv { get; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!HostingEnv.IsDevelopment()) context.Result = new NotFoundResult();
        }
    }
}