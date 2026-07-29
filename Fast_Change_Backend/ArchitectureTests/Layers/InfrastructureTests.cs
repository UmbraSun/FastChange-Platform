using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests.Layers;

public sealed class InfrastructureTests
{
    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Core()
    {
        var result = Types.InAssembly(typeof(Infrastructure.AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Core")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
