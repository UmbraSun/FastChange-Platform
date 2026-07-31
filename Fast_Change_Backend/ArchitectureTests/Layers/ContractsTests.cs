using Contracts;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests.Layers;

public sealed class ContractsTests
{
    [Fact]
    public void Contracts_Should_Not_Depend_On_Application()
    {
        var result = Types
            .InAssembly(typeof(AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Application",
                "Infrastructure",
                "Core")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
