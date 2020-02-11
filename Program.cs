using System;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {
        var pid = GetPidUsingPInvoke();
        Console.WriteLine($"My PID from P/Invoke: {pid}");
        pid = GetPidUsingNativeLibrary();
        Console.WriteLine($"My PID from NativeLibrary: {pid}");
        pid = GetPidUsingMonoLibrary();
        Console.WriteLine($"My PID from Mono.Posix.NETStandard: {pid}");
    }
    [DllImport("libc", EntryPoint = "getpid")]
    private static extern int GetPidUsingPInvoke();
    private delegate int GetPidDelegate();
    private static int GetPidUsingNativeLibrary()
    {
        IntPtr libraryPtr = NativeLibrary.Load("libc");
        IntPtr symbolPtr = NativeLibrary.GetExport(libraryPtr, "getpid");
        var getPidDelegate = Marshal.GetDelegateForFunctionPointer<GetPidDelegate>(symbolPtr);
        return getPidDelegate();
    }
    private static int GetPidUsingMonoLibrary() => Mono.Unix.Native.Syscall.getpid();
}
