using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using IDust.Base;

namespace IDust.Vision.Tools.CodeScan;
public class QrCodeAction : CodeActionBase
{
    public HDataCode2D handle;

    public QrCodeAction()
    {
        this.handle = new HDataCode2D();
        this.CodeType = CodeType.QrCode_MatrixCode;
    }

    public override RunResult<List<CodeScanResult>> Run(ref HImage image)
    {
        return base.Run(ref image);
    }
}

