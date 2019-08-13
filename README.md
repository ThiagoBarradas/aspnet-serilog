[![Build status](https://ci.appveyor.com/api/projects/status/kk6x66pwp6x4u0y0/branch/master?svg=true)](https://ci.appveyor.com/project/ThiagoBarradas/aspnet-serilog/branch/master)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1370d7b2cca34f3e90d07257707f657b)](https://www.codacy.com/app/ThiagoBarradas/aspnet-serilog?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ThiagoBarradas/aspnet-serilog&amp;utm_campaign=Badge_Grade)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AspNetSerilog.svg)](https://www.nuget.org/packages/AspNetSerilog/)
[![NuGet Version](https://img.shields.io/nuget/v/AspNetSerilog.svg)](https://www.nuget.org/packages/AspNetSerilog/)

# AspNetSerilog

Serilog logger for AspNet Core web applications. Handler request, response and exceptions.

# Sample

Configure service in statup
```c#
// Startup.cs

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
