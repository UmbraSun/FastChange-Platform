using Application.Common.Interfaces;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests.Layers;

public sealed class ApplicationTests
{
    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types.InAssembly(typeof(IUnitOfWork).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
