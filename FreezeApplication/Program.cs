using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreezeApplication
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hHandle);

        private enum ThreadAccess
        {
            SUSPEND_RESUME = 0x0002
        }

        static void Main(string[] args)
        {
            Console.Write("Which process do you want to stop (PID): ");

            int pid;

            if (int.TryParse(Console.ReadLine(), out pid))
            {
                try
                {
                    SuspendProcess(pid);
                    Console.WriteLine($"Process {pid} has been suspended.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid PID.");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static void SuspendProcess(int pid)
        {
            var process = Process.GetProcessById(pid);
            foreach (ProcessThread thread in process.Threads)
            {
                var tHandle = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (tHandle != IntPtr.Zero)
                {
                    SuspendThread(tHandle);
                    CloseHandle(tHandle);
                }
            }
        }

        public static void ResumeProcess(int pid)
        {
            var process = Process.GetProcessById(pid);
            foreach (ProcessThread thread in process.Threads)
            {
                var tHandle = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (tHandle != IntPtr.Zero)
                {
                    ResumeThread(tHandle);
                    CloseHandle(tHandle);
                }
            }
        }
    }
}
