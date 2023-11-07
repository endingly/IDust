using System;
using HalconDotNet;
using IDust.Vision.Tools.CodeScan;
using Xunit.Abstractions;

namespace IDust.Test.Vision.CodeScan;

public class TestCodeTool : TestBase, IDisposable
{
    private ScanCodeTool codeTool;

    public TestCodeTool(ITestOutputHelper tempOutput) : base(tempOutput)
    {
        codeTool = new ScanCodeTool();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Test_BarCode()
    {
        HImage image = new($"{Environment.CurrentDirectory}/Test_Data/barcode_test.jpg");
        HImage image2 = new($"{Environment.CurrentDirectory}/Test_Data/barcode_test.bmp");
        var r = codeTool.Run(ref image);
        HOperatorSet.WriteImage(image, "jpg", 0, $"{Environment.CurrentDirectory}/Test_Data/barcode_test_result.jpg");
        
        var r2 = codeTool.Run(ref image2);
        HOperatorSet.WriteImage(image2, "jpg", 0, $"{Environment.CurrentDirectory}/Test_Data/barcode_test_result1.jpg");
        if (r.Content != null)
        {
            r.Content.ForEach(x =>
            {
                Output.WriteLine(x.DataString);
            });
        }
        if (r2.Content != null)
        {
            r2.Content.ForEach(x =>
            {
                Output.WriteLine(x.DataString);
            });
        }
    }
}