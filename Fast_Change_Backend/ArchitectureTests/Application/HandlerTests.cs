using Application;
using Application.Common.Interfaces;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests.Application;

public sealed class HandlerTests
{
    [Fact]
    public void Handlers_Should_Be_Sealed()
    {
        var result = Types.InAssembly(typeof(AssemblyReference).Assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_Reside_In_Features()
    {
        var result = Types.InAssembly(typeof(AssemblyReference).Assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .ResideInNamespaceStartingWith("Application.Features")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_Should_Reside_In_Features()
    {
        var result = Types.InAssembly(typeof(IUnitOfWork).Assembly)
            .That()
            .HaveNameEndingWith("Validator")
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespaceStartingWith("Application.Features")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Depend_On_External_Libraries()
    {
        var result = Types.InAssembly(typeof(AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Confluent.Kafka",
                "MongoDB.Driver",
                "StackExchange.Redis",
                "Qdrant.Client",
                "OpenAI")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
