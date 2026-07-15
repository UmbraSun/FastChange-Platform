using Domain.Entities;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests.Layers;

public sealed class DomainTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Other_Layers()
    {
        var result = Types.InAssembly(typeof(User).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Application",
                "Infrastructure",
                "Core")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
