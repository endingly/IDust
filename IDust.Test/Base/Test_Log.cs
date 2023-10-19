using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDust.Base;
using Xunit.Abstractions;

namespace IDust.Test.Base;

public class Test_Log
{
    [Fact]
    public void Test_Log_Info()
    {
        var logger = Log.GetLogger(LogKeyword.Camera);
        logger.Fatal("Test_Log_Info");
    }
}

