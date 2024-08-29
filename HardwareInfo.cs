#if IOS

using ObjCRuntime;
using System;
using System.Runtime.InteropServices;

#endif

using System.Runtime.InteropServices;

namespace SfMAUI;

public class HardwareInfo
{
    /// <summary>
    /// The get memory usage current process.
    /// </summary>
    /// <returns>
    /// The <see cref="double"/>.
    /// </returns>
    public static double GetMemoryUsageCurrentProcess()
    {
        try
        {
            long bytes;

            // GetCurrentProcess() is not supported in net8.0 ios or android
            // As such we have had to implement an iOS-only means of getting
            // the current memory usage. The below should be reverted when this
            // is resolved (seems to be with .NET 9). We should also remove
            // the .net8-ios target from TargetFrameworks in csproj.
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                bytes = MemoryManager.GetCurrentlyUsedMemoryInBytes();
            }
            else
            { 
               bytes = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            }

            if (bytes > 0)
            {
                var megabytes = (bytes / 1024f) / 1024f;
                return megabytes;
            }
        }
        catch (System.Exception)
        {
            // do nothing.
        }
        return 0;
    }
}


// GetCurrentProcess() is not supported in net8.0 ios or android
// As such we have had to implement this iOS-only class to get
// the current memory usage. The below should be removed when this
// is resolved (seems to be with .NET 9). We should also remove
// the .net8-ios target from TargetFrameworks in csproj. 

public static class MemoryManager
{
    const int KERN_SUCCESS = 0;
    const int MACH_TASK_BASIC_INFO = 20;

    static IntPtr self;
    static mach_task_basic_info tbi = new mach_task_basic_info();
    static int size = Marshal.SizeOf(typeof(mach_task_basic_info));

    static MemoryManager()
    {
        // open lib, ensure it gets included by linker
#if IOS
        var handle = Dlfcn.dlopen("/usr/lib/libSystem.dylib", 0);
        self = Dlfcn.GetIntPtr(handle, "mach_task_self_");
        Dlfcn.dlclose(handle);

#endif
    }
    
    // fetches this method from the libSystem.dylib linked dll
    [DllImport("/usr/lib/libSystem.dylib")]
    extern static /* kern_return_t */ int task_info(
        /* task_name_t -> mach_port_t */ IntPtr target_task,
        /* task_flavor_t -> natural_t */ int flavor,
        /* task_info_t -> integer_t* */ ref mach_task_basic_info task_info_out,
        /* mach_msg_type_number_t* -> natural_t* */ ref int task_info_outCnt);

    struct mach_task_basic_info
    {
        public long /* mach_vm_size_t */ virtual_size;       /* virtual memory size (bytes) */
        public long /* mach_vm_size_t */ resident_size;      /* resident memory size (bytes) */
        public long /* mach_vm_size_t */ resident_size_max;  /* maximum resident memory size (bytes) */
        public long /* time_value_t */ user_time;          /* total user run time for terminated threads */
        public long /* time_value_t */ system_time;        /* total system run time for terminated threads */
        public int /* policy_t */ policy;             /* default policy for new threads */
        public int /* integer_t */ suspend_count;      /* suspend count for task */
    };
    
    private static mach_task_basic_info? GetResidentSize()
    {
        var err = task_info(self, MACH_TASK_BASIC_INFO, ref tbi, ref size);
        return (err == KERN_SUCCESS) ? tbi : default(mach_task_basic_info?);
    }

    public static long GetCurrentlyUsedMemoryInBytes()
    {
        var memoryInfo = GetResidentSize();

        return memoryInfo.HasValue 
            ? memoryInfo.Value.resident_size 
            : -1;
    }
}
