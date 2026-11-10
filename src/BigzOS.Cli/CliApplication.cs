using System.Globalization;
using System.Reflection;
using System.Text.Json;
using BigzOS.Core;

namespace BigzOS.Cli;

public static class CliApplication
{
    private const string Usage = "Uso: bigz info [--json] | bigz --version";

    public static int Run(string[] args, TextWriter output, TextWriter error,
        bool isWindows, Func<ISystemInformationSource> sourceFactory)
    {
        if (args.Length == 0 || args is ["--help"] or ["-h"])
        {
            output.WriteLine($"Bigz Toolkit | {Usage}");
            return 0;
        }
        if (args is ["--version"])
        {
            var version = typeof(CliApplication).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
            output.WriteLine(version.Split('+', 2)[0]);
            return 0;
        }

        bool json = args is ["info", "--json"];
        if (!json && args is not ["info"])
        {
            error.WriteLine($"Comando inválido. {Usage}");
            return 2;
        }
        if (!isWindows)
        {
            error.WriteLine("O comando info requer Windows.");
            return 1;
        }

        var info = new SystemInformationCollector(sourceFactory()).Collect();
        if (json)
        {
            output.WriteLine(JsonSerializer.Serialize(new
            {
                windowsVersion = info.WindowsVersion.Value,
                architecture = info.Architecture.Value,
                processor = info.Processor.Value,
                totalMemoryBytes = info.TotalMemoryBytes.Available ? info.TotalMemoryBytes.Value : (ulong?)null
            }));
        }
        else
        {
            WriteText(info, output);
        }

        return info.WindowsVersion.Available && info.Architecture.Available
            && info.Processor.Available && info.TotalMemoryBytes.Available ? 0 : 1;
    }

    private static void WriteText(SystemInformation info, TextWriter output)
    {
        output.WriteLine($"Windows: {Display(info.WindowsVersion)}");
        output.WriteLine($"Arquitetura: {Display(info.Architecture)}");
        output.WriteLine($"Processador: {Display(info.Processor)}");
        output.WriteLine("Memória física total: " + (info.TotalMemoryBytes.Available
            ? (info.TotalMemoryBytes.Value / 1073741824d).ToString("F2", CultureInfo.InvariantCulture) + " GiB"
            : "indisponível"));
    }

    private static string Display(InformationField<string> field) =>
        field.Available ? field.Value! : "indisponível";
}
