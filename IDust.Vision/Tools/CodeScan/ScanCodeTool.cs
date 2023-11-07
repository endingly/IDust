using HalconDotNet;
using IDust.Base;
using IDust.Vision.Tools.CodeScan;
using System.Text;
using System.Text.Json;

namespace IDust.Vision.Tools.CodeScan;
using ScanResultPackages = List<CodeScanResult>;

public class ScanCodeTool : ToolBase
{
    private ScanCodeToolParma parma;

    public ScanCodeTool()
    {
        parma = new ScanCodeToolParma();
        ToolName = parma.ToolName;
    }
    
    public ScanCodeTool(ScanCodeToolParma parma)
    {
        this.parma = parma;
        ToolName = parma.ToolName;
    }

    

    /// <summary>
    /// 构造函数，指明参数文件的保存路径，解析并赋值
    /// </summary>
    /// <param name="parmaFileSavePath">参数文件保存路径</param>
    public ScanCodeTool(string parmaFileSavePath)
    {
        var r = JsonSerializer.Deserialize<ScanCodeToolParma>(parmaFileSavePath);
        if (r != null)
        {
            parma = r;
            ToolName = parma.ToolName;
        }
        else
        {
            parma = new ScanCodeToolParma();
            Log.GetLogger(LogKeyword.VisionTool).Warn(ErrorCode.VisionToolFailToInit.GetString());
        }
        if (!Directory.Exists(ToolWorkDir))
        {
            Directory.CreateDirectory(ToolWorkDir);
        }
    }

    public override RunResult Save()
    {
        try
        {
            var st = File.Open(ParmaSaveFilePath + ToolName + ".json", FileMode.OpenOrCreate | FileMode.Truncate);
            JsonSerializer.Serialize(st, parma);
            st.DisposeAsync();
            return new RunResult(ErrorCode.VisionToolSaveParmaSuccess);
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.VisionToolFailToSaveParma, ex);
        }
    }

    public override RunResult<ScanResultPackages> Run(ref HImage image, InParma? inParma = null, OutParma? outParma = null)
    {
        ScanResultPackages results = [];
        if (!image.IsInitialized())
        {
            return new RunResult<ScanResultPackages>(ErrorCode.VisionToolImageInvalid);
        }
        else
        {
            foreach (var item in this.parma.Actions)
            {
                var r = item.Run(ref image);
                if (r.Content != null)
                {
                    results.AddRange(r.Content);
                }
                else
                {
                    return new RunResult<ScanResultPackages>(ErrorCode.VisionToolFailToRun);
                }
            }
            return new RunResult<ScanResultPackages>(ErrorCode.VisionToolRunSuccess, results);
        }
    }
}