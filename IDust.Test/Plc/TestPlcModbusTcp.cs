using IDust.Communicate.Plc;
using HslCommunication.ModBus;
using Xunit.Abstractions;
using HslCommunication.Core;

namespace IDust.Test.Plc
{
    public class TestPlcModbusTcp : TestBase
    {
        public TestPlcModbusTcp(ITestOutputHelper tempOutput) : base(tempOutput)
        {
        }

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
            server.ServerClose();
            Assert.True(rcs.isSuccess && rcc.isSuccess);
        }

        [Fact]
        public void Test_Read()
        {
            PlcParma parma = new PlcParma();
            parma.IpAddress = "127.0.0.1";
            parma.Port = 502;
            ModbusTcpServer server = new ModbusTcpServer()
            {
                Port = parma.Port,
                DataFormat = parma.DataFormat,
                IsStringReverse = parma.StrReverse,
                Station = (byte)parma.Station
            };
            server.ServerStart();
            var op = server.Write("1000", 1);

            PlcModbusTcp client = new PlcModbusTcp(in parma);
            var rcs = client.ConnectServer();
            var r = client.ReadWriteHandle.ReadValue("1000", out int value);

            client.ConnectClose();
            server.ServerClose();
            Assert.True(op.IsSuccess && r.isSuccess && (value == 1));
        }

        [Fact]
        public void Test_Write()
        {
            PlcParma parma = new PlcParma();
            parma.IpAddress = "127.0.0.1";
            parma.Port = 502;
            ModbusTcpServer server = new ModbusTcpServer()
            {
                Port = parma.Port,
                DataFormat = parma.DataFormat,
                IsStringReverse = parma.StrReverse,
                Station = (byte)parma.Station
            };
            server.ServerStart();
            

            PlcModbusTcp client = new PlcModbusTcp(in parma);
            var rcs = client.ConnectServer();
            var r = client.ReadWriteHandle.WriteValue("1000", 1);
            var op = server.ReadInt32("1000", 1);

            client.ConnectClose();
            server.ServerClose();
            Assert.True(op.IsSuccess && r.isSuccess && (op.Content[0] == 1));
        }
    }
}
