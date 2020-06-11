
[![NuGet Downloads](https://img.shields.io/nuget/dt/AspNetSerilog.svg)](https://www.nuget.org/packages/AspNetSerilog/)
[![NuGet Version](https://img.shields.io/nuget/v/AspNetSerilog.svg)](https://www.nuget.org/packages/AspNetSerilog/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ThiagoBarradas_aspnet-serilog&metric=alert_status)](https://sonarcloud.io/dashboard?id=ThiagoBarradas_aspnet-serilog)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ThiagoBarradas_aspnet-serilog&metric=coverage)](https://sonarcloud.io/dashboard?id=ThiagoBarradas_aspnet-serilog)

# AspNetSerilog

Serilog logger for AspNet Core web applications. Handler request, response and exceptions.

# Sample

Configure your Startup.cs

```c#

public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    var config = new SerilogConfiguration
    {
        Blacklist = new string[] { "password", "credit_card" }
    };

    services.SetupSerilog(config);
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseAspNetSerilog();
    app.UseMvc();
}
```

Ready! That way all request/response will be sended to serilog.

You can custom information title / error title and Serilog Logger using SerilogConfiguration in constructor. By default, global serilog logger will be used.

You can disable logging on success using DisableSerilogExtension in your action:

```c#

[HttpGet("")]
public IActionResult Home()
{
	this.DisableLogging();
	...
}

```

For exception log, in your global exception handler, add exception in context items, like:

```
context.Items.Add("Exception", exception);
```

## Properties 

* `RequestBody`
* `Method`
* `Path`
* `Host`
* `Port`
* `Url`
* `QueryString`
* `Query`
* `RequestHeaders`
* `Ip`
* `IsSuccessful`
* `StatusCode`
* `StatusDescription`
* `StatusCodeFamily`
* `ProtocolVersion`
* `ErrorException`
* `ErrorMessage`
* `ResponseContent`
* `ContentType`
* `ContentLength`
* `ResponseHeaders`
* `ElapsedMilliseconds`
* `RequestKey`

You can use this propeties with serilog log context to build log messages. `HTTP {Method} {Path} {...}`.

## Install via NuGet

```
PM> Install-Package AspNetSerilog
```

## How can I contribute?
Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?
Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Changelog
See in [nuget version history](https://www.nuget.org/packages/AspNetSerilog)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
