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
        readonly Guid _iid;
        readonly RPC_VERSION _version;

        /// <summary>
        /// Populates the instance
        /// </summary>
        public RpcInterface(Guid IID, ushort MajorVersion, ushort MinorVersion)
        {
            this._iid = IID;
            this._version.MajorVersion = MajorVersion;
            this._version.MinorVersion = MinorVersion;
        }

        internal Guid IID { get { return _iid; } }
        internal RPC_VERSION VERSION { get { return _version; } }
    }
}
