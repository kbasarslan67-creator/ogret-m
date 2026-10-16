using System.Reflection;
using NetArchTest.Rules;
using TipEgitim.Application;
using TipEgitim.Infrastructure;
using TipEgitim.SharedKernel;

namespace TipEgitim.ArchitectureTests;

/// <summary>Bağımlılık kuralı: bağımlılıklar yalnızca içe doğru akar (ADR-001).</summary>
public sealed class LayerDependencyTests
{
    private const string ApplicationNamespace = "TipEgitim.Application";
    private const string InfrastructureNamespace = "TipEgitim.Infrastructure";
    private const string WebNamespace = "TipEgitim.Web";

    private static readonly Assembly _sharedKernel = typeof(Result).Assembly;
    private static readonly Assembly _application = typeof(IDispatcher).Assembly;
    private static readonly Assembly _infrastructure = typeof(ServiceCollectionExtensions).Assembly;

    [Fact]
    public void SharedKernelDoesNotDependOnOtherLayers()
    {
        AssertNoDependency(_sharedKernel, ApplicationNamespace, InfrastructureNamespace, WebNamespace);
    }

    [Fact]
    public void ApplicationDoesNotDependOnInfrastructureOrWeb()
    {
        AssertNoDependency(_application, InfrastructureNamespace, WebNamespace);
    }

    [Fact]
    public void InfrastructureDoesNotDependOnWeb()
    {
        AssertNoDependency(_infrastructure, WebNamespace);
    }

    private static void AssertNoDependency(Assembly assembly, params string[] forbiddenNamespaces)
    {
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenNamespaces)
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Yasak bağımlılık: " + string.Join(", ", result.FailingTypeNames ?? []));
    }
}
