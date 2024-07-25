using Board.Application.Options;
using Board.Application.Repositories;
using Board.Application.Services.Files.Services;
using Board.Application.Users.CommandHandlers;
using Board.Infrastructure.Context;
using Board.Infrastructure.Repositories;
using Board.Infrastructure.Services.Files;
using Board.WebApi.ExceptionHandlers;
using Board.WebApi.ExceptionHandlers.Factory;
using Board.WebApi.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Board.WebApi.Extensions;

internal static class WebApplicationBuilderExtensions
{
    internal static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        builder.Services
            .AddDbContext<DataContext>(options =>
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration["DbConnection"]);
                dataSourceBuilder.EnableDynamicJson();

                options.UseNpgsql(
                    dataSourceBuilder.Build(),
                    providerOptions => providerOptions.EnableRetryOnFailure());

                options.UseSnakeCaseNamingConvention();
            })
            .AddScoped<DbContext>(sp => sp.GetRequiredService<DataContext>());

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();

        builder.Services.AddImageService(builder.Configuration);
        builder.Services.AddBoardOptions(builder.Configuration);

        builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));

        builder.Services.AddSingleton<ExceptionHandler>();
        builder.Services.AddSingleton<IExceptionResponseBuilderFactory>(provider =>
        {
            var environment = provider.GetRequiredService<IWebHostEnvironment>();
            return new ExceptionResponseBuilderFactory(environment.IsDevelopment());
        });

        return builder;
    }

    private static void AddImageService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StaticPhotoServiceOption>()
            .Bind(configuration.GetSection(StaticPhotoServiceOption.Name));
        services.AddScoped<IStaticPhotoServiceOption>(
            provider => provider.GetRequiredService<IOptions<StaticPhotoServiceOption>>().Value);

        services.AddScoped<IPhotoService, StaticPhotoService>();
    }

    private static void AddBoardOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BoardOption>()
            .Bind(configuration.GetSection(BoardOption.Name));
        services.AddScoped<IBoardOption>(
            provider => provider.GetRequiredService<IOptions<BoardOption>>().Value);
    }
}
