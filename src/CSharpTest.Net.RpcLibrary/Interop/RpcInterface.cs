using CSharpTest.Net.RpcLibrary.Interop;
using CSharpTest.Net.RpcLibrary.Interop.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Net.RpcLibrary
{
    /// <summary>
    /// Describes a versioned RPC interface
    /// </summary>
    public class RpcInterface
    {
        internal static readonly byte[] DEFAULT_TYPE_FORMAT;
        internal static readonly byte[] DEFAULT_FUNC_FORMAT;

        static RpcInterface()
        {
            if (RpcUtils.Is64BitProcess())
            {
                DEFAULT_TYPE_FORMAT = new byte[]
                    {
                        0x00, 0x00, 0x1b, 0x00, 0x01, 0x00, 0x28, 0x00, 0x08, 0x00,
                        0x01, 0x00, 0x01, 0x5b, 0x11, 0x0c, 0x08, 0x5c, 0x11, 0x14,
                        0x02, 0x00, 0x12, 0x00, 0x02, 0x00, 0x1b, 0x00, 0x01, 0x00,
                        0x28, 0x54, 0x18, 0x00, 0x01, 0x00, 0x01, 0x5b, 0x00
                    };
                DEFAULT_FUNC_FORMAT = new byte[]
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
                DEFAULT_TYPE_FORMAT = new byte[]
                    {
                        0x00, 0x00, 0x1b, 0x00, 0x01, 0x00, 0x28, 0x00, 0x04, 0x00,
                        0x01, 0x00, 0x01, 0x5b, 0x11, 0x0c, 0x08, 0x5c, 0x11, 0x14,
                        0x02, 0x00, 0x12, 0x00, 0x02, 0x00, 0x1b, 0x00, 0x01, 0x00,
                        0x28, 0x54, 0x0c, 0x00, 0x01, 0x00, 0x01, 0x5b, 0x00
                    };
                DEFAULT_FUNC_FORMAT = new byte[]
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
        public static RpcInterface Default(Guid IID)
        {
            return new RpcInterface(IID, 1, 0, 1, DEFAULT_TYPE_FORMAT, DEFAULT_FUNC_FORMAT);
        }

        readonly Guid _IID;
        readonly RPC_VERSION _VERSION;
        readonly ushort _METHOD_COUNT;
        readonly byte[] _TYPE_FORMAT;
        readonly byte[] _FUNC_FORMAT;
        readonly Ptr<Byte[]> _FUNC_FORMAT_PTR;


        /// <summary>
        /// Populates the instance
        /// </summary>
        public RpcInterface(Guid IID, ushort MajorVersion, ushort MinorVersion, ushort MethodCount,
            byte[] TypeFormat, byte[] FuncFormat)
        {
            this._IID = IID;
            this._VERSION.MajorVersion = MajorVersion;
            this._VERSION.MinorVersion = MinorVersion;
            this._METHOD_COUNT = MethodCount;
            this._TYPE_FORMAT = TypeFormat;
            this._FUNC_FORMAT = FuncFormat;
            this._FUNC_FORMAT_PTR = new Ptr<byte[]>(FUNC_FORMAT);


        }

        internal Guid IID { get { return _IID; } }
        internal RPC_VERSION VERSION { get { return _VERSION; } }
        internal ushort METHOD_COUNT { get { return _METHOD_COUNT; } }
        internal byte[] TYPE_FORMAT {  get {  return _TYPE_FORMAT; } }
        internal byte[] FUNC_FORMAT { get { return _FUNC_FORMAT; } }
        internal Ptr<byte[]> FUNC_FORMAT_PTR { get { return _FUNC_FORMAT_PTR; } }


    }
}
