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
    internal struct RPC_CLIENT_INTERFACE
    {
        uint Length;
        RPC_SYNTAX_IDENTIFIER InterfaceId;
        RPC_SYNTAX_IDENTIFIER TransferSyntax;
        IntPtr /*PRPC_DISPATCH_TABLE*/ DispatchTable;
        uint RpcProtseqEndpointCount;
        IntPtr /*PRPC_PROTSEQ_ENDPOINT*/ RpcProtseqEndpoint;
        IntPtr Reserved;
        IntPtr InterpreterInfo;
        uint Flags;

        public static readonly Guid IID_SYNTAX = new Guid(0x8A885D04u, 0x1CEB, 0x11C9, 0x9F, 0xE8, 0x08, 0x00, 0x2B,
                                                          0x10,
                                                          0x48, 0x60);

        public static RPC_CLIENT_INTERFACE FromRpcInterface(RpcInterface @interface)
        {
            var result = new RPC_CLIENT_INTERFACE();
            result.Length = (uint) Marshal.SizeOf(typeof (RPC_CLIENT_INTERFACE));
            result.InterfaceId = new RPC_SYNTAX_IDENTIFIER() {SyntaxGUID = @interface.IID, SyntaxVersion = @interface.VERSION };
            result.TransferSyntax = new RPC_SYNTAX_IDENTIFIER()
                                 {SyntaxGUID = IID_SYNTAX, SyntaxVersion = RPC_VERSION.SYNTAX_VERSION};
            result.DispatchTable = IntPtr.Zero;
            result.RpcProtseqEndpointCount = 0u;
            result.RpcProtseqEndpoint = IntPtr.Zero;
            result.Reserved = IntPtr.Zero;
            result.InterpreterInfo = IntPtr.Zero;
            result.Flags = 0u;
            return result;
        }
    }
}