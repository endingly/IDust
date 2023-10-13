using HslCommunication.ModBus;
using HslCommunication.Profinet.Delta;
using IDust.Communicate.Plc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace IDust.Test.Plc
{
    public class TestPlcDeltaModbusTcp : TestBase, IDisposable
    {
        private ModbusTcpServer server;
        private PlcDeltaModbusTcp client;

        public TestPlcDeltaModbusTcp(ITestOutputHelper tempOutput) : base(tempOutput)
        {
            PlcParma parma = new PlcParma();
            parma.IpAddress = "127.0.0.1";
            parma.Port = 502;

            server = new ModbusTcpServer()
            {
                Port = parma.Port,
                DataFormat = parma.DataFormat,
                IsStringReverse = parma.StrReverse,
                Station = (byte)parma.Station
            };
            client = new PlcDeltaModbusTcp(parma);
            server.ServerStart(502);
        }

        public void Dispose()
        {
            server.ServerClose();
            server.Dispose();
            client.ConnectClose();
        }

        [Fact]
        public void Test_ConnectClose()
        {
            Assert.True(client.ConnectOpen().isSuccess);
        }

        [Fact]
        public void Test_Read()
        {
            var op = server.Write("1000", 1);
            var r = client.ReadWriteHandle.ReadValue("1000", out int value);
            Assert.True(op.IsSuccess && r.isSuccess && (value == 1));
        }

        [Fact]
        public void Test_WriteAndClear()
        {
            // 测试写入
            var r = client.ReadWriteHandle.WriteValue("1000", 1);
            var op = server.ReadInt32("1000", 1);
            Assert.True(op.IsSuccess && r.isSuccess && (op.Content[0] == 1));
            // 测试清空
            var cop = client.ReadWriteHandle.ClearValue("1000", 4);
            var cop2 = server.ReadInt32("1000", 1);
            Assert.True(cop.isSuccess && cop2.IsSuccess);
            Output.WriteLine(cop2.Content[0].ToString("X"));
        }
    }
}
