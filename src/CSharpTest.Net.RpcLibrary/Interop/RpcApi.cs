#region Copyright 2010-2014 by Roger Knapp, Licensed under the Apache License, Version 2.0
/* Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion
using System;
using System.Runtime.InteropServices;
using CSharpTest.Net.RpcLibrary.Interop.Structs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CSharpTest.Net.RpcLibrary.Interop
{
    /// <summary>
    /// WinAPI imports for RPC
    /// </summary>
    public static class RpcApi
    {
        #region Memory Utils

        [DllImport("Kernel32.dll", EntryPoint = "LocalFree", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr LocalFree(IntPtr memHandle);

        public static void Free(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                Log.Verbose("LocalFree({0})", ptr);
                LocalFree(ptr);
            }
        }

        private const UInt32 LPTR = 0x0040;

        [DllImport("Kernel32.dll", EntryPoint = "LocalAlloc", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr LocalAlloc(UInt32 flags, UInt32 nBytes);

        public static IntPtr Alloc(uint size)
        {
            IntPtr ptr = LocalAlloc(LPTR, size);
            Log.Verbose("{0} = LocalAlloc({1})", ptr, size);
            return ptr;
        }

        [DllImport("Rpcrt4.dll", EntryPoint = "NdrServerCall2", CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern void NdrServerCall2(IntPtr ptr);

        internal delegate void ServerEntryPoint(IntPtr ptr);

        internal static FunctionPtr<ServerEntryPoint> ServerEntry = new FunctionPtr<ServerEntryPoint>(NdrServerCall2);

        internal static FunctionPtr<LocalAlloc> AllocPtr = new FunctionPtr<LocalAlloc>(Alloc);
        internal static FunctionPtr<LocalFree> FreePtr = new FunctionPtr<LocalFree>(Free);

        #endregion
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
