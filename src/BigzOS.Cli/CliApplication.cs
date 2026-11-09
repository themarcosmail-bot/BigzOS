using System.Globalization;
using BigzOS.Core;

namespace BigzOS.Cli;

public static class CliApplication
{
    public static int Run(string[] args, TextWriter output, TextWriter error,
        bool isWindows, Func<ISystemInformationSource> sourceFactory)
    {
        if (args.Length == 0 || args is ["--help"] or ["-h"])
        {
            output.WriteLine("Bigz Toolkit | Uso: bigz info");
            return 0;
        }
        if (args is not ["info"])
        {
            error.WriteLine("Comando inválido. Uso: bigz info");
            return 2;
        }
        if (!isWindows)
        {
            error.WriteLine("O comando info requer Windows.");
            return 1;
        }

        var info = new SystemInformationCollector(sourceFactory()).Collect();
        output.WriteLine($"Windows: {Display(info.WindowsVersion)}");
        output.WriteLine($"Arquitetura: {Display(info.Architecture)}");
        output.WriteLine($"Processador: {Display(info.Processor)}");
        output.WriteLine("Memória física total: " + (info.TotalMemoryBytes.Available
            ? (info.TotalMemoryBytes.Value / 1073741824d).ToString("F2", CultureInfo.InvariantCulture) + " GiB"
            : "indisponível"));
        return info.WindowsVersion.Available && info.Architecture.Available
            && info.Processor.Available && info.TotalMemoryBytes.Available ? 0 : 1;
    }

    private static string Display(InformationField<string> field) =>
        field.Available ? field.Value! : "indisponível";
}
