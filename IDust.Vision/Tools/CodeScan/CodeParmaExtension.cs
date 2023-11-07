using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Tools.CodeScan;
public static class CodeParmaExtension
{
    public static string GetString(this CodeType t)
    {
        return t switch
        {
            CodeType.BarCode_Code128 => "Code 128",
            CodeType.QrCode_MatrixCode => "Matrix Code",
            _ => "auto"
        };
    }
}

