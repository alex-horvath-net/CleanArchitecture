using Common.Business;
using Experts.Trader.ReadTransactions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.ReadTransactions;

public class ReadTest {
    RepositoryAdapterPlugin CreateUnit() => new(dependencies.RepositoryTechnologyPlugin);
    Task<List<Transaction>> UseTheUnit(RepositoryAdapterPlugin unit) => unit.ReadTransaction(arguments.Request, arguments.Token);
    Dependencies dependencies = Dependencies.Default();
    Arguments arguments = Arguments.Some();


    [Fact]
    public async Task It_Should_Find_Somthing() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeNull();
    }

    [Fact]
    public async Task It_Should_Find_List_Of_Transaction() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().BeOfType<List<Transaction>>();
    }

    [Fact]
    public async Task It_Should_Find_All_Transaction() {
        var unit = CreateUnit();
        arguments = Arguments.All();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeEmpty();
    }

    [Fact]
    public async Task It_Should_Find_Some_Transaction() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().NotBeEmpty();
    }

    [Fact]
    public async Task It_Should_Find_Some_Matching_Transaction() {
        var unit = CreateUnit();
        var transactions = await UseTheUnit(unit);
        transactions.Should().AllSatisfy(x => x.Name.Contains(arguments.Request.Name));
    }

    [Fact]
    public async Task It_Should_Not_Find_Non_Matching_Transactions() {
        var unit = CreateUnit();
        arguments = Arguments.Nothing();
        var transactions = await UseTheUnit(unit);
        transactions.Should().BeEmpty();
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
        services.AddRead(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var repositoryAdapterPort = serviceProvider.GetService<Feature.IRepositoryAdapterPort>();
        var repositoryTechnologyPort = serviceProvider.GetService<RepositoryAdapterPlugin.RepositoryTechnologyPort>();
        var dbContext = serviceProvider.GetService<Common.Technology.AppData.AppDB>();

        repositoryAdapterPort.Should().NotBeNull();
        repositoryTechnologyPort.Should().NotBeNull();
        dbContext.Should().NotBeNull();
    }


    public record Dependencies(
        RepositoryTechnologyPlugin RepositoryTechnologyPlugin) {

        public static Dependencies Default() {
            var databaseFactory = new DatabaseFactory();
            var entityFramework = databaseFactory.Default();
            var repositoryTechnologyPlugin = new RepositoryTechnologyPlugin(entityFramework);

            return new Dependencies(repositoryTechnologyPlugin);
        }
    }

    public record Arguments(Feature.Request Request, CancellationToken Token) {
        public static Arguments All() => new(
          new() { Name = null },
          CancellationToken.None);

        public static Arguments Some() => new(
            new() { Name = "USD" },
            CancellationToken.None);

        public static Arguments Nothing() => new(
          new() { Name = "USD_Typo" },
          CancellationToken.None);
    }
}
