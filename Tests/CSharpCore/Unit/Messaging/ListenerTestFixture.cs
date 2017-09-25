/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/6/2010
 * Time: 3:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Messaging
{
    /// <summary>
    /// Description of ListenerTestFixture.
    /// </summary>
    public class ListenerTestFixture
    {
        [Fact]
        public void AddBindingDuplicate()
        {
            Assert.Equal(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            var listener = new Listener();
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            Assert.Equal(1, listener.Bindings.Count);
        }
        
        [Fact]
        public void RemoveBinding()
        {
            Assert.Equal(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            var listener = new Listener();
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            listener.RemoveBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            Assert.Equal(0, listener.Bindings.Count);
        }

        [Fact]
        public void Restart()
        {
            Assert.Equal(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            var listener = new Listener();
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            Assert.False(listener.Active);
            listener.Start();
            Assert.True(listener.Active);
            listener.Stop();
            Assert.False(listener.Active);
            listener.Start();
            Assert.True(listener.Active);
            listener.Stop();
            Assert.False(listener.Active);
        }

        [Fact]
        public void DoubleStart()
        {
            Assert.Equal(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            var listener = new Listener();
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            Assert.False(listener.Active);
            listener.Start();
            Assert.True(listener.Active);
            listener.Start();
            Assert.True(listener.Active);
            listener.Stop();
            Assert.False(listener.Active);
        }

        [Fact]
        public void DoubleStop()
        {
            Assert.Equal(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            var listener = new Listener();
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            Assert.False(listener.Active);
            listener.Start();
            Assert.True(listener.Active);
            listener.Stop();
            Assert.False(listener.Active);
            listener.Stop();
            Assert.False(listener.Active);
        }

        [Fact]
        public void InitialStop()
        {
            Assert.Equal(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            var listener = new Listener();
            listener.AddBinding(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21));
            Assert.False(listener.Active);
            listener.Stop();
            Assert.False(listener.Active);
        }
    }
}
