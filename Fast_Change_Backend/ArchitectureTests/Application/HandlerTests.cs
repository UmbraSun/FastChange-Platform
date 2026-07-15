using Application;
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
        var result = Types.InAssembly(typeof(AssemblyReference).Assembly)
            .That()
            .HaveNameEndingWith("Validator")
            .Should()
            .ResideInNamespaceStartingWith("Application.Features")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
