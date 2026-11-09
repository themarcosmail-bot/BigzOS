using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace BigzOS.Core;

[SupportedOSPlatform("windows")]
public sealed class WindowsInformationSource : ISystemInformationSource
{
    public string ReadWindowsVersion() => RuntimeInformation.OSDescription;
    public string ReadArchitecture() => RuntimeInformation.OSArchitecture.ToString();

    public string ReadProcessor()
    {
        // Explicitly read-only access to hardware description.
        using var key = Registry.LocalMachine.OpenSubKey(
            @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", writable: false);
        return (key?.GetValue("ProcessorNameString") as string)?.Trim()
            ?? throw new InvalidOperationException("Processor unavailable.");
    }

    public ulong ReadTotalMemoryBytes()
    {
        var status = new MemoryStatus { Length = (uint)Marshal.SizeOf<MemoryStatus>() };
        if (!GlobalMemoryStatusEx(ref status))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        if (status.TotalPhysical == 0)
            throw new InvalidOperationException("Physical memory unavailable.");
        return status.TotalPhysical;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatus status);

    [StructLayout(LayoutKind.Sequential)]
    private struct MemoryStatus
    {
        public uint Length;
        public uint MemoryLoad;
        public ulong TotalPhysical;
        public ulong AvailablePhysical;
        public ulong TotalPageFile;
        public ulong AvailablePageFile;
        public ulong TotalVirtual;
        public ulong AvailableVirtual;
        public ulong AvailableExtendedVirtual;
    }
}
