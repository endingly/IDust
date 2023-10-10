using IDust.Communicate.Plc;
using HslCommunication.ModBus;

namespace IDust.Test.Plc
{
    public class TestPlcModbusTcp
    {
        [Fact]
        public void Test_ConnectAndClose()
        {
            ModbusTcpServer server = new ModbusTcpServer();
            server.ServerStart(502);

            PlcParma parma = new PlcParma();
            parma.IpAddress = "127.0.0.1";
            parma.Port = 502;
            PlcModbusTcp client = new PlcModbusTcp(in parma);
            var rcs = client.ConnectServer();
            var rcc = client.ConnectClose();
            Assert.True(rcs.isSuccess && rcc.isSuccess);
        }
    }
}
