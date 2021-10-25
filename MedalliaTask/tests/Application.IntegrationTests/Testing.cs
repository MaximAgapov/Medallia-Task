using System;
using MedalliaTask.Infrastructure.Persistence;
using MedalliaTask.WebUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System.IO;
using System.Threading.Tasks;
using MedalliaTask.Domain.Entities;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private ApplicationDbContext _context;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "MedalliaTask.WebUI"));

        services.AddLogging();

        startup.ConfigureServices(services);

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    private static async Task Seed(ApplicationDbContext context)
    {
        context.Orders.Add(new Order());

        var apple = new ShopItem { Name = "Apple", Price = 0.50 }; 
        var banana = new ShopItem { Name = "Banana", Price = 0.70 }; 
        var oranges = new ShopItem { Name = "Oranges", Price = 0.45 }; 
        var ananas = new ShopItem { Name = "Ananas", Price = 1.00 }; 

        
        context.ShopItems.AddRange(apple, banana, oranges, ananas);
        
        var offer = new SpecialOffer { Amount = 2, Price = 1, ShopItem = banana};
        var offerBest = new SpecialOffer { Amount = 3, Price = 0.9, ShopItem = oranges};
            
        context.SpecialOffer.AddRange(offer, offerBest);
       
        await context.SaveChangesAsync();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        Action<ApplicationDbContext> extraSeed = null, bool needSeed = true)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (needSeed)
        {
            context.Database.Migrate();
            await Seed(context);
        }

        extraSeed?.Invoke(context);
        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task<ApplicationDbContext> AddAsync<TEntity>(TEntity entity, ApplicationDbContext context)
        where TEntity : class
    {
        context.Add(entity);

        await context.SaveChangesAsync();
        return context;
    }
    
    public static async Task<ApplicationDbContext> AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        return await AddAsync(entity, context);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}