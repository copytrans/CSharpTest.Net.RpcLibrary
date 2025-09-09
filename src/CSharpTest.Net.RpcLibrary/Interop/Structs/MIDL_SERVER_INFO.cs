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

namespace CSharpTest.Net.RpcLibrary.Interop.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIDL_SERVER_INFO
    {
        public IntPtr /* PMIDL_STUB_DESC */ pStubDesc;
        public IntPtr /* SERVER_ROUTINE* */ DispatchTable;
        public IntPtr /* PFORMAT_STRING */ ProcString;
        public IntPtr /* unsigned short* */ FmtStringOffset;
        private IntPtr /* STUB_THUNK * */ ThunkTable;
        private IntPtr /* PRPC_SYNTAX_IDENTIFIER */ pTransferSyntax;
        private IntPtr /* ULONG_PTR */ nCount;
        private IntPtr /* PMIDL_SYNTAX_INFO */ pSyntaxInfo;

        internal static MIDL_SERVER_INFO FromRpcInterface(RpcHandle handle, RpcInterface @interface, Ptr<RPC_SERVER_INTERFACE> serverInterfacePtr, RpcExecute fnExecute)
        {
            MIDL_SERVER_INFO result = new MIDL_SERVER_INFO();
            MIDL_STUB_DESC StubDesc = MIDL_STUB_DESC.FromRpcServerInterface(handle, @interface, serverInterfacePtr);
            result.pStubDesc = handle.Pin(StubDesc);
            var DispatchTable = new IntPtr[@interface.METHOD_COUNT];
            for (var i = 0; i < DispatchTable.Length; i++)
            {
                DispatchTable[i] = handle.PinFunction(fnExecute);
            }
            result.DispatchTable = handle.Pin(DispatchTable);
            result.ProcString = handle.Pin(@interface.FUNC_FORMAT);
            result.FmtStringOffset = handle.Pin(@interface.FUNC_FORMAT_OFFSETS);
            result.ThunkTable = IntPtr.Zero;
            result.pTransferSyntax = IntPtr.Zero;
            result.nCount = IntPtr.Zero;
            result.pSyntaxInfo = IntPtr.Zero;
            return result;
        }

    }

    internal delegate uint RpcExecute(
        IntPtr clientHandle, uint szInput, IntPtr input, out uint szOutput, out IntPtr output);
}