using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Tools;
public static class ScanCodeParmaExtension
{
    public static string GetString(this BarCodeType t)
    {
        return t switch
        {
            BarCodeType.Code128 => "Code 128",

            _ => "Code 128"
        };
    }
}

