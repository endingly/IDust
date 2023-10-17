using IDust.Communicate;
using IDust.Communicate.LightController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace IDust.Test.Naive
{
    public class TestInStruct : TestBase
    {
        SerialPortParma parma;

        public TestInStruct(ITestOutputHelper tempOutput) : base(tempOutput)
        {
            parma = new SerialPortParma();
        }

        [Fact]
        public void Test_InKeyworkds()
        {
            LightControllerBase lightController = new LightControllerBase(parma);
            this.parma.DataBits = 1;

            Assert.Equal(7, lightController.serialPortParma.DataBits);
        }
    }
}
