using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Net.RpcLibrary.Interop
{

    /// <summary>
    /// Various utilities required for RPC 
    /// </summary>
    public abstract class RpcUtils
    {

        /// <summary>
        /// Checks whether current process is 64 bits
        /// </summary>
        public static bool Is64BitProcess() { return (IntPtr.Size == 8); }


    }
}
