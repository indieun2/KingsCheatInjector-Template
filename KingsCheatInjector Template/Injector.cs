using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KingsCheatInjector_Template
{
    public class Injector // This is what flags your antivirus
    {
        [DllImport("kernel32.dll")] static extern IntPtr OpenProcess(int access, bool inherit, int pid);
        [DllImport("kernel32.dll")] static extern IntPtr VirtualAllocEx(IntPtr proc, IntPtr addr, uint size, uint type, uint protect);
        [DllImport("kernel32.dll")] static extern bool WriteProcessMemory(IntPtr proc, IntPtr addr, byte[] buffer, uint size, out UIntPtr written);
        [DllImport("kernel32.dll")] static extern IntPtr GetProcAddress(IntPtr hMod, string name);
        [DllImport("kernel32.dll")] static extern IntPtr GetModuleHandle(string name);
        [DllImport("kernel32.dll")] static extern IntPtr CreateRemoteThread(IntPtr proc, IntPtr attr, uint size, IntPtr start, IntPtr param, uint flags, IntPtr threadId);

        const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 0x04;

        public static void Inject(int pid, string dllPath)
        {
            IntPtr procHandle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
            IntPtr allocMem = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)dllPath.Length + 1, MEM_COMMIT, PAGE_READWRITE);

            byte[] dllBytes = Encoding.ASCII.GetBytes(dllPath);
            WriteProcessMemory(procHandle, allocMem, dllBytes, (uint)dllBytes.Length, out _);

            IntPtr loadLibAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibAddr, allocMem, 0, IntPtr.Zero);
        }
    }
}