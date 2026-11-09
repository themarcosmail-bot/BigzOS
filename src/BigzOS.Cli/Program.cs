using BigzOS.Cli;
using BigzOS.Core;

return CliApplication.Run(args, Console.Out, Console.Error,
    OperatingSystem.IsWindows(), CreateSource);

static ISystemInformationSource CreateSource()
{
    if (!OperatingSystem.IsWindows())
        throw new PlatformNotSupportedException();
    return new WindowsInformationSource();
}

