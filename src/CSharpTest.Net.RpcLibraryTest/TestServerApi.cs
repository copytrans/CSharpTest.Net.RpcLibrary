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
using System.Collections.Generic;
using static CSharpTest.Net.RpcLibrary.BuiltinRpcInterface;

namespace CSharpTest.Net.RpcLibrary.Test
{
    [TestClass]
    public class TestServerApi
    {
        [TestMethod]
        public void TestUnregisterListener()
        {
            Guid iid = Guid.NewGuid();
            BuiltinRpcInterface @interface = BuiltinRpcInterface.Default(iid);
            using (RpcServerApi server = new RpcServerApi(@interface))
            {
                server.AddProtocol(RpcProtseq.ncalrpc, "lrpctest", 5);
                server.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_WINNT);
                server.StartListening();
                RpcExecutor handler =
                    delegate(IRpcClientInfo client, byte[] arg)
                    { return arg; };

                using (RpcClientApi client = new RpcClientApi(BuiltinRpcInterface.Default(iid), RpcProtseq.ncalrpc, null, "lrpctest"))
                {
                    client.AuthenticateAs(null, RpcClientApi.Self, RpcProtectionLevel.RPC_C_PROTECT_LEVEL_PKT_PRIVACY, RpcAuthentication.RPC_C_AUTHN_WINNT);

                    @interface.OnExecute += handler;
                    client.Execute(new byte[0]);

                    @interface.OnExecute -= handler;
                    try 
                    {
                        client.Execute(new byte[0]);
                        Assert.Fail();
                    }
                    catch (RpcException)
                    { }
                }
            }
        }

        [TestMethod]
        public void TestVerboseLog()
        {
            RpcServerApi.VerboseLogging = true;
            Assert.IsTrue(RpcServerApi.VerboseLogging);

            RpcServerApi.VerboseLogging = false;
            Assert.IsFalse(RpcServerApi.VerboseLogging);
        }
    }
}
