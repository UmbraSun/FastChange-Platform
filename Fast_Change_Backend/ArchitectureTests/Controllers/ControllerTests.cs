using Core.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetArchTest.Rules;

namespace ArchitectureTests.Controllers;

public sealed class ControllerTests
{
    [Fact]
    public void Controllers_Should_Use_ISender()
    {
        var result = Types.InAssembly(typeof(AuthController).Assembly)
            .That()
            .Inherit(typeof(ControllerBase))
            .ShouldNot()
            .HaveDependencyOn(typeof(IMediator).FullName!)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}