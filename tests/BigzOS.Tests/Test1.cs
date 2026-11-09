using BigzOS.Cli;
using BigzOS.Core;
namespace BigzOS.Tests;

[TestClass]
public sealed class InformationTests
{
    [TestMethod]
    public void CollectorReturnsInjectedValues()
    {
        var result = new SystemInformationCollector(new FakeSource()).Collect();
        Assert.AreEqual("Windows fixture", result.WindowsVersion.Value);
        Assert.AreEqual("Arm64", result.Architecture.Value);
        Assert.AreEqual("CPU fixture", result.Processor.Value);
        Assert.AreEqual(8589934592UL, result.TotalMemoryBytes.Value);
        Assert.IsTrue(result.TotalMemoryBytes.Available);
    }

    [TestMethod]
    public void FailedFieldDoesNotHideOtherFields()
    {
        var result = new SystemInformationCollector(new FakeSource { FailProcessor = true }).Collect();
        Assert.IsFalse(result.Processor.Available);
        Assert.IsTrue(result.WindowsVersion.Available);
        Assert.IsTrue(result.TotalMemoryBytes.Available);
    }

    [TestMethod]
    public void EmptyFieldIsUnavailable()
    {
        Assert.IsFalse(new SystemInformationCollector(new FakeSource { Version = " " })
            .Collect().WindowsVersion.Available);
    }

    [TestMethod]
    public void InfoFormatsInjectedInformation()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        Assert.AreEqual(0, CliApplication.Run(["info"], output, error, true, () => new FakeSource()));
        StringAssert.Contains(output.ToString(), "Memória física total: 8.00 GiB");
        StringAssert.Contains(output.ToString(), "Processador: CPU fixture");
        Assert.AreEqual("", error.ToString());
    }

    [TestMethod]
    public void PartialFailureHasNonzeroExitAndNoExceptionDetails()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        Assert.AreEqual(1, CliApplication.Run(["info"], output, error, true,
            () => new FakeSource { FailProcessor = true }));
        StringAssert.Contains(output.ToString(), "Processador: indisponível");
        Assert.IsFalse(output.ToString().Contains("secret", StringComparison.Ordinal));
        Assert.AreEqual("", error.ToString());
        Assert.IsFalse(error.ToString().Contains("secret", StringComparison.Ordinal));
    }

    [TestMethod]
    public void MemoryFailurePreservesOtherFieldsAndCompletesOutput()
    {
        var source = new FakeSource { FailMemory = true };
        var result = new SystemInformationCollector(source).Collect();
        Assert.IsFalse(result.TotalMemoryBytes.Available);
        Assert.IsTrue(result.WindowsVersion.Available);
        Assert.IsTrue(result.Architecture.Available);
        Assert.IsTrue(result.Processor.Available);

        using var output = new StringWriter();
        using var error = new StringWriter();
        Assert.AreEqual(1, CliApplication.Run(["info"], output, error, true, () => source));
        StringAssert.Contains(output.ToString(), "Windows: Windows fixture");
        StringAssert.Contains(output.ToString(), "Arquitetura: Arm64");
        StringAssert.Contains(output.ToString(), "Processador: CPU fixture");
        StringAssert.Contains(output.ToString(), "Memória física total: indisponível");
        Assert.IsFalse(output.ToString().Contains("secret", StringComparison.Ordinal));
        Assert.AreEqual("", error.ToString());
    }

    [TestMethod]
    public void InvalidArgumentsNeverCollect()
    {
        Assert.AreEqual(2, CliApplication.Run(["info", "extra"], TextWriter.Null,
            TextWriter.Null, true, NeverCreate));
    }

    [TestMethod]
    public void UnsupportedPlatformNeverCollects()
    {
        Assert.AreEqual(1, CliApplication.Run(["info"], TextWriter.Null,
            TextWriter.Null, false, NeverCreate));
    }

    [TestMethod]
    public void HelpNeverCollects()
    {
        Assert.AreEqual(0, CliApplication.Run(["--help"], TextWriter.Null,
            TextWriter.Null, true, NeverCreate));
    }

    private static ISystemInformationSource NeverCreate() =>
        throw new AssertFailedException("Unexpected collection");

    private sealed class FakeSource : ISystemInformationSource
    {
        public bool FailProcessor { get; init; }
        public bool FailMemory { get; init; }
        public string Version { get; init; } = "Windows fixture";
        public string ReadWindowsVersion() => Version;
        public string ReadArchitecture() => "Arm64";
        public string ReadProcessor() => FailProcessor
            ? throw new UnauthorizedAccessException("secret") : "CPU fixture";
        public ulong ReadTotalMemoryBytes() => FailMemory
            ? throw new InvalidOperationException("secret memory failure") : 8589934592UL;
    }
}
