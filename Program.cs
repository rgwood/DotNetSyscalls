using System;
using System.Runtime.InteropServices;

namespace syscalls
{
    class Program
    {
        static void Main()
        {
            var pid = GetPidUsingPInvoke();
            Console.WriteLine($"My PID from P/Invoke: {pid}");

            pid = GetPidUsingNativeLibrary();
            Console.WriteLine($"My PID from NativeLibrary: {pid}");

            pid = Mono.Unix.Native.Syscall.getpid();
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
    }
}
