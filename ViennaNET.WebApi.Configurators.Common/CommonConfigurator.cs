using ViennaNET.Logging;
using ViennaNET.Logging.Configuration;
using ViennaNET.WebApi.Abstractions;
using ViennaNET.WebApi.Configurators.Common.Middleware;
using ViennaNET.WebApi.Net;
using ViennaNET.WebApi.Net.IpTools;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Hosting;

namespace ViennaNET.WebApi.Configurators.Common
{
  /// <summary>
  /// Включает базовые сервисы и middleware
  /// </summary>
  public static class CommonConfigurator
  {
    /// <summary>
    /// Включает базовые сервисы и middleware
    /// </summary>
    /// <param name="companyHostBuilder"></param>
    /// <returns></returns>
    public static ICompanyHostBuilder UseCommonModules(this ICompanyHostBuilder companyHostBuilder)
    {
      return companyHostBuilder.AddHostBuilderAction(AddLogger)
                               .ConfigureApp(AddLogForRequests)
                               .ConfigureApp(ConfigureCompanyMiddleware)
                               .RegisterServices(RegisterCommonServices);
    }

    /// <summary>
    /// Регистрирует сервисы для работы с IP-адресами
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    internal static void RegisterCommonServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddSingleton<ILocalIpProvider, LocalIpProvider>();
      services.AddSingleton<ILoopbackIpFilter, LoopbackIpFilter>();
    }

    /// <summary>
    /// Добавляет Middleware для логирования запросов и ответов сервиса
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    /// <param name="container"></param>
    internal static void AddLogForRequests(
      IApplicationBuilder builder, IConfiguration configuration, IHostEnvironment env, object container)
    {
      builder.UseSerilogRequestLogging();
    }

    /// <summary>
    /// Добавляет Middleware для логирования запросов и ответов сервиса
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    /// <param name="container"></param>
    internal static void AddLogger(IWebHostBuilder builder, IConfiguration configuration)
    {
      builder.UseSerilog((ctx, sc) =>
      {
        sc.MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console();
      });
    }

    /// <summary>
    /// Регистрирует базовые middleware в приложении и DI контейнере
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    /// <param name="container"></param>
    internal static void ConfigureCompanyMiddleware(
      IApplicationBuilder builder, IConfiguration configuration, IHostEnvironment env, object container)
    {
      builder.UseMiddleware<RequestRegistrationMiddleware>();
      //builder.UseMiddleware<SetUpLoggerMiddleware>();
      //builder.UseMiddleware<LogRequestAndResponseMiddleware>();
    }
  }
}
