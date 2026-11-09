namespace BigzOS.Core;

public interface ISystemInformationSource
{
    string ReadWindowsVersion();
    string ReadArchitecture();
    string ReadProcessor();
    ulong ReadTotalMemoryBytes();
}

public sealed record InformationField<T>(T? Value, bool Available);
public sealed record SystemInformation(
    InformationField<string> WindowsVersion,
    InformationField<string> Architecture,
    InformationField<string> Processor,
    InformationField<ulong> TotalMemoryBytes);

public sealed class SystemInformationCollector(ISystemInformationSource source)
{
    public SystemInformation Collect() => new(
        Read(source.ReadWindowsVersion), Read(source.ReadArchitecture),
        Read(source.ReadProcessor), Read(source.ReadTotalMemoryBytes));

    private static InformationField<T> Read<T>(Func<T> read)
    {
        try
        {
            var value = read();
            return value is null || value is string text && string.IsNullOrWhiteSpace(text)
                ? new(default, false) : new(value, true);
        }
        catch (Exception exception) when (exception is not OutOfMemoryException)
        {
            // Independent field failures must not prevent the remaining reads.
            return new(default, false);
        }
    }
}
