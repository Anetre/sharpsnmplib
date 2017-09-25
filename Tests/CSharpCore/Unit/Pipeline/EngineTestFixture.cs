using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Net;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Pipeline
{
    public class EngineTestFixture
    {
        static NumberGenerator port = new NumberGenerator(50000, 55000);

        private SnmpEngine CreateEngine()
        {
            // TODO: this is a hack. review it later.
            var store = new ObjectStore();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());
            store.Add(new IfNumber());
            store.Add(new IfTable());

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
#if NET452
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));
#endif
            var getv1 = new GetV1MessageHandler();
            var getv1Mapping = new HandlerMapping("v1", "GET", getv1);

            var getv23 = new GetMessageHandler();
            var getv23Mapping = new HandlerMapping("v2,v3", "GET", getv23);

            var setv1 = new SetV1MessageHandler();
            var setv1Mapping = new HandlerMapping("v1", "SET", setv1);

            var setv23 = new SetMessageHandler();
            var setv23Mapping = new HandlerMapping("v2,v3", "SET", setv23);

            var getnextv1 = new GetNextV1MessageHandler();
            var getnextv1Mapping = new HandlerMapping("v1", "GETNEXT", getnextv1);

            var getnextv23 = new GetNextMessageHandler();
            var getnextv23Mapping = new HandlerMapping("v2,v3", "GETNEXT", getnextv23);

            var getbulk = new GetBulkMessageHandler();
            var getbulkMapping = new HandlerMapping("v2,v3", "GETBULK", getbulk);

            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1, v2, v3 });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                getv1Mapping,
                getv23Mapping,
                setv1Mapping,
                setv23Mapping,
                getnextv1Mapping,
                getnextv23Mapping,
                getbulkMapping
            });

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            var listener = new Listener { Users = users };
            listener.ExceptionRaised += (sender, e) => { Assert.True(false, "unexpected exception"); };
            listener.MessageReceived += (sender, e) => { Console.WriteLine($"{DateTime.Now.ToString("o")} agent received"); };
            return new SnmpEngine(pipelineFactory, listener, new EngineGroup());
        }

        [Fact]
        public void Restart()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(serverEndPoint);

            Assert.False(engine.Active);
            engine.Start();
            Assert.True(engine.Active);
            engine.Stop();
            Assert.False(engine.Active);
            engine.Start();
            Assert.True(engine.Active);
            engine.Stop();
            Assert.False(engine.Active);
        }

        [Fact]
        public void DoubleStart()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(serverEndPoint);

            Assert.False(engine.Active);
            engine.Start();
            Assert.True(engine.Active);
            engine.Start();
            Assert.True(engine.Active);
            engine.Stop();
            Assert.False(engine.Active);
        }

        [Fact]
        public void DoubleStop()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(serverEndPoint);

            Assert.False(engine.Active);
            engine.Start();
            Assert.True(engine.Active);
            engine.Stop();
            Assert.False(engine.Active);
            engine.Stop();
            Assert.False(engine.Active);
        }

        [Fact]
        public void InitialStop()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(serverEndPoint);

            Assert.False(engine.Active);
            engine.Stop();
            Assert.False(engine.Active);
        }
    }
}
