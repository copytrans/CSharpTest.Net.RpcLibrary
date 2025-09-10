using CSharpTest.Net.RpcLibrary.Interop;
using CSharpTest.Net.RpcLibrary.Interop.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static CSharpTest.Net.RpcLibrary.RpcServerApi;

namespace CSharpTest.Net.RpcLibrary
{
    /// <summary>
    /// Describes a versioned RPC interface
    /// </summary>
    public abstract class RpcInterface
    {

        readonly Guid _IID;
        readonly RPC_VERSION _VERSION;
        IntPtr[] _FUNCTIONS = null;
        readonly byte[] _FUNCTION_FORMATS;
        readonly ushort[] _FUNCTION_FORMAT_OFFSETS;
        readonly byte[] _TYPE_FORMATS;


        /// <summary>
        /// Populates the instance
        /// </summary>
        public RpcInterface(Guid IID, ushort MajorVersion, ushort MinorVersion,
            byte[] FunctionFormats, ushort[] FunctionFormatOffsets, byte[] TypeFormats)
        {
            this._IID = IID;
            this._VERSION.MajorVersion = MajorVersion;
            this._VERSION.MinorVersion = MinorVersion;
            this._FUNCTION_FORMATS = FunctionFormats;
            this._FUNCTION_FORMAT_OFFSETS = FunctionFormatOffsets;
            this._TYPE_FORMATS = TypeFormats;
        }

        /// <summary>
        /// Returns the list of function pointers for the interface.
        /// </summary>
        protected abstract IntPtr[] Functions(RpcHandle handle);

        internal Guid IID { get { return _IID; } }
        internal RPC_VERSION VERSION { get { return _VERSION; } }
        internal IntPtr[] FUNCTIONS(RpcHandle handle)
        {
            if (_FUNCTIONS == null)
                _FUNCTIONS = Functions(handle);
            return _FUNCTIONS;
        }
        internal byte[] FUNCTION_FORMATS { get { return _FUNCTION_FORMATS; } }
        internal ushort[] FUNCTION_FORMAT_OFFSETS { get { return _FUNCTION_FORMAT_OFFSETS; } }
        internal byte[] TYPE_FORMATS { get { return _TYPE_FORMATS; } }


    }

    /// <summary>
    /// An RPC interface that supports bassing bytes in and out
    /// </summary>
    public class BuiltinRpcInterface : RpcInterface
    {
        /// <summary>
        /// The delegate format for the OnExecute event
        /// </summary>
        public delegate byte[] RpcExecutor(IRpcClientInfo client, byte[] input);
        private delegate uint RpcFunction(IntPtr clientHandle, uint szInput, IntPtr input, out uint szOutput, out IntPtr output);

        internal static readonly byte[] BUILTIN_TYPE_FORMATS;
        internal static readonly byte[] BUILTIN_FUNCTION_FORMATS;

        static BuiltinRpcInterface()
        {
            if (RpcUtils.Is64BitProcess())
            {
                BUILTIN_TYPE_FORMATS = new byte[]
                    {
                        0x00, 0x00, 0x1b, 0x00, 0x01, 0x00, 0x28, 0x00, 0x08, 0x00,
                        0x01, 0x00, 0x01, 0x5b, 0x11, 0x0c, 0x08, 0x5c, 0x11, 0x14,
                        0x02, 0x00, 0x12, 0x00, 0x02, 0x00, 0x1b, 0x00, 0x01, 0x00,
                        0x28, 0x54, 0x18, 0x00, 0x01, 0x00, 0x01, 0x5b, 0x00
                    };
                BUILTIN_FUNCTION_FORMATS = new byte[]
                    {
                        0x00, 0x68, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x00,
                        0x32, 0x00, 0x00, 0x00, 0x08, 0x00, 0x24, 0x00, 0x47, 0x05,
                        0x0a, 0x07, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x48, 0x00, 0x08, 0x00, 0x08, 0x00, 0x0b, 0x00, 0x10, 0x00,
                        0x02, 0x00, 0x50, 0x21, 0x18, 0x00, 0x08, 0x00, 0x13, 0x20,
                        0x20, 0x00, 0x12, 0x00, 0x70, 0x00, 0x28, 0x00, 0x10, 0x00,
                        0x00
                    };
            }
            else
            {
                BUILTIN_TYPE_FORMATS = new byte[]
                    {
                        0x00, 0x00, 0x1b, 0x00, 0x01, 0x00, 0x28, 0x00, 0x04, 0x00,
                        0x01, 0x00, 0x01, 0x5b, 0x11, 0x0c, 0x08, 0x5c, 0x11, 0x14,
                        0x02, 0x00, 0x12, 0x00, 0x02, 0x00, 0x1b, 0x00, 0x01, 0x00,
                        0x28, 0x54, 0x0c, 0x00, 0x01, 0x00, 0x01, 0x5b, 0x00
                    };
                BUILTIN_FUNCTION_FORMATS = new byte[]
                    {
                        0x00, 0x68, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00,
                        0x32, 0x00, 0x00, 0x00, 0x08, 0x00, 0x24, 0x00, 0x47, 0x05,
                        0x08, 0x07, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x48, 0x00,
                        0x04, 0x00, 0x08, 0x00, 0x0b, 0x00, 0x08, 0x00, 0x02, 0x00,
                        0x50, 0x21, 0x0c, 0x00, 0x08, 0x00, 0x13, 0x20, 0x10, 0x00,
                        0x12, 0x00, 0x70, 0x00, 0x14, 0x00, 0x10, 0x00, 0x00
                    };
            }
        }

        /// <summary>
        /// Provides a default instance for agnostic C# only RPC
        /// </summary>
        public static BuiltinRpcInterface Default(Guid IID)
        {
            return new BuiltinRpcInterface(IID, 1, 0, null);
        }

        /// <summary>
        /// Provides a default instance for agnostic C# only RPC
        /// </summary>
        public static BuiltinRpcInterface Default(Guid IID, RpcExecutor executor)
        {
            return new BuiltinRpcInterface(IID, 1, 0, executor);
        }

        private RpcExecutor _executor = null;


        private BuiltinRpcInterface(Guid iid, ushort major, ushort minor, RpcExecutor executor)
            : base(iid, major, minor, BUILTIN_FUNCTION_FORMATS, new ushort[] { 0 }, BUILTIN_TYPE_FORMATS)
        {
            _executor = executor;
        }

        /// <summary>
        /// Returns the list of function pointers for the interface.
        /// </summary>
        override protected IntPtr[] Functions(RpcHandle handle)
        {
            return new IntPtr[] { handle.PinFunction< RpcFunction>(RpcEntryPoint) };
        }

        private uint RpcEntryPoint(IntPtr clientHandle, uint szInput, IntPtr input, out uint szOutput, out IntPtr output)
        {
            output = IntPtr.Zero;
            szOutput = 0;

            try
            {
                byte[] bytesIn = new byte[szInput];
                Marshal.Copy(input, bytesIn, 0, bytesIn.Length);

                byte[] bytesOut;
                using (RpcClientInfo client = new RpcClientInfo(clientHandle))
                {
                    bytesOut = _executor == null ? null : _executor(client, bytesIn);
                }
                if (bytesOut == null)
                {
                    return (uint)RpcError.RPC_S_NOT_LISTENING;
                }

                szOutput = (uint)bytesOut.Length;
                output = RpcApi.Alloc(szOutput);
                Marshal.Copy(bytesOut, 0, output, bytesOut.Length);

                return (uint)RpcError.RPC_S_OK;
            }
            catch (Exception ex)
            {
                RpcApi.Free(output);
                output = IntPtr.Zero;
                szOutput = 0;

                Log.Error(ex);
                return (uint)RpcError.RPC_E_FAIL;
            }
        }

        /// <summary>
        /// Allows a single subscription to this event to handle incomming requests rather than 
        /// deriving from and overriding the Execute call.
        /// </summary>
        public event RpcExecutor OnExecute
        {
            add
            {
                lock (this)
                {
                    Check.Assert<InvalidOperationException>(_executor == null, "The interface id is already registered.");
                    _executor = value;
                }
            }
            remove
            {
                lock (this)
                {
                    Check.NotNull(value);
                    if (_executor != null)
                        Check.Assert<InvalidOperationException>(
                            Object.ReferenceEquals(_executor.Target, value.Target)
                            && Object.ReferenceEquals(_executor.Method, value.Method)
                            );
                    _executor = null;
                }
            }
        }
    }
}
