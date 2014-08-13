SSW.Owin.AppClaimsModule
========================

Owin Module for providing your own application claims to the OwinContext Claims Principal.

## Usage

If you are using Forms Authentication (with the new ASP.NET Identity), you should already have a `Startup.cs` file in your MVC or WebApi project.

If not, add one to the root of your project with the contents below:

```csharp
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSW.Owin.AppClaimsModule.Example.Startup))]
namespace SSW.Owin.AppClaimsModule.Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
```

If you have a `Startup.Auth.cs` file in your `App_Start` folder, add one of the lines below to the `ConfigureAuth` method:

```csharp
app.UseAppClaimsIdentityProvider(new ExampleClaimsProvider());
```
or
```csharp
app.UseAppClaimsAdditiveProvider(new ExampleClaimsProvider());
```

If you don't have a `Startup.Auth.cs` file in your `App_Start` folder, add one with the following method, and put one of the above lines in the `ConfigureAuth` method.

```csharp
public partial class Startup
{
    public void ConfigureAuth(IAppBuilder app)
    {
        // put one of the above lines in here
    }
}
```

## App Builder Extension Methods

There are two extension methods provided for the Owin App Builder.  The only difference is whether the additional claims get added to the existing Identity, or whether a new Identity is added to the authenticated User object.

The `UseAppClaimsIdentityProvder` will add a new Identity. The `UseAppClaimsAdditiveProvider` will add the new claims to the authenticated User Identity.

## AppClaimsProvider

You will need to create a class that inherits the `AppClaimsProvider` abstract class. This class should override the `GetClaimsForPrincipal` method which should return an `IEnumerable` of `Claim` objects.

You can find an example of this class in the `SSW.Owin.AppClaimsModule.Example` project.

## ClaimsCacheProvider

This optional argument lets you provide a cache for these claims to avoid retrieving claims from a data source on every request.

We provide a default `ClaimsCacheProvider` which uses `HttpRuntime.Cache`. You can instantiate this yourself or use the static `ClaimsCacheProvider.DefaultCacheProvider` method which defaults to a 30min absolute expiry time.
