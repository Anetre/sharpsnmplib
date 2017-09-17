using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class StreamExtensionTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => StreamExtension.AppendBytes(null, SnmpType.Counter32, null, null));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.IgnoreBytes(null, 0));
#if !NETCOREAPP2_0
            Assert.Throws<ArgumentNullException>(() => new MemoryStream().AppendBytes(SnmpType.Counter32, null, null));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.ReadPayloadLength(null));
#endif
        }
    }
}
