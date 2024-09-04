using Experts.Trader.ReadTransactions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.ReadTransactions;

public class FeatureTest
{
    Feature CreateUnit() => new(
        dependencies.Validator,
        dependencies.Repository);
    Task<Feature.Response> UseTheUnit(Feature unit) => unit.Execute(arguments.Request, arguments.Token);
    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Valid();

    [Fact]
    public async Task Response_Should_NotBeNull()
    {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Response_Request_Should_NotBeNull()
    {
        var unit = CreateUnit();
        var response = await UseTheUnit(unit);
        response.Request.Should().NotBeNull(); 
    }

    [Fact]
    public async Task Response_Errors_Should_Reflect_Validation_Issues()
    {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var response = await UseTheUnit(unit);
        response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Response_Transactions_Should_BeEmpty_If_There_Is_Validation_Issues() {
        var unit = CreateUnit();
        arguments = Arguments.InValid();
        var response = await UseTheUnit(unit);
        response.Transactions.Should().BeEmpty();
    }


    [Fact]
    public void AddRead_ShouldRegisterDependencies() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?> {
            { "ConnectionStrings:App", "Data Source=.\\SQLEXPRESS;Initial Catalog=App;User ID=sa;Password=sa!Password;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False" }
        });
        // Act
        services.AddReadTransactions(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var feature = serviceProvider.GetService<Feature>();

        feature.Should().NotBeNull();
    }


    public record Dependencies(
        Feature.IValidatorAdapterPort Validator,
        Feature.IRepositoryAdapterPort Repository)
    {

        public static Dependencies Default()
        {
            //var repository = Substitute.For<Feature.IRepository>();
            //repository.Read(default).Returns([]);
            var fluentValidator = new ValidatorTechnologyPlugin.RequestValidator();
            var validatorTechnologyPlugin = new ValidatorTechnologyPlugin(fluentValidator);
            var validatorAdapterPlugin = new ValidatorAdapterPlugin(validatorTechnologyPlugin);

            var databaseFactory = new DatabaseFactory();
            var entityFramework = databaseFactory.Default();
            var repositoryTechnologyPlugin = new RepositoryTechnologyPlugin(entityFramework);
            var repositoryAdapterPlugin = new RepositoryAdapterPlugin(repositoryTechnologyPlugin);

            return new Dependencies(validatorAdapterPlugin, repositoryAdapterPlugin);
        }
    }

    public record Arguments(Feature.Request Request, CancellationToken Token)
    {
        public static Arguments Valid() => new(
            new() { Name = "USD" },
            CancellationToken.None);

        public static Arguments InValid() => new(
          new() { Name = "US" },
          CancellationToken.None);
    }
}