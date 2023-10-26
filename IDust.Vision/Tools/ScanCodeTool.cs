using HalconDotNet;
using IDust.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace IDust.Vision.Tools;

public class ScanCodeTool : ToolBase
{
    public HTuple[] BarHandles = new HTuple[3];
    public HTuple? DataCodeHandle;
    private ScanCodeToolParma parma;

    public ScanCodeTool(ScanCodeToolParma parma)
    {
        this.parma = parma;
    }

    public override string ToolName { get => parma.ToolName; set => parma.ToolName = value; }

    /// <summary>
    /// 构造函数，指明参数文件的保存路径，解析并赋值
    /// </summary>
    /// <param name="parmaFileSavePath">参数文件保存路径</param>
    public ScanCodeTool(string parmaFileSavePath)
    {
        var r = JsonSerializer.Deserialize<ScanCodeToolParma>(parmaFileSavePath);
        if (r != null)
        {
            this.parma = r;
        }
        else
        {
            this.parma = new ScanCodeToolParma();
            Log.GetLogger(LogKeyword.VisionTool).Error(ErrorCode.VisionToolFailToInit.GetString());
        }
        if (!Directory.Exists(this.ToolWorkDir))
        {
            Directory.CreateDirectory(this.ToolWorkDir);
        }
    }

    public override RunResult Init()
    {
        try
        {
            if (DataCodeHandle != null)
            {
                HOperatorSet.ClearDataCode2dModel(DataCodeHandle);
            }
            if (BarHandles[0] != null)
            {
                for (int i = 0; i < 3; i++)
                    HOperatorSet.ClearBarCodeModel(BarHandles[i]);
            }
            if (this.parma.CodeType == CodeType.BarCode)
            {
                for (int i = 0; i < 3; i++)
                    HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out BarHandles[i]);
            }
            else if (this.parma.CodeType == CodeType.QRCode)
            {
                HOperatorSet.CreateDataCode2dModel(new HTuple(), new HTuple(), new HTuple(), out DataCodeHandle);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out BarHandles[i]);
                HOperatorSet.CreateDataCode2dModel(new HTuple(), new HTuple(), new HTuple(), out DataCodeHandle);
            }
            var r = CodeToolParmaInit();
            return r.isSuccess ? new RunResult(ErrorCode.VisionToolInitSuccess) : r;
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.VisionToolFailToInit, ex);
        }
        
    }

    public override RunResult Save()
    {
        try
        {
            var st = File.Open(ParmaSaveName + ToolName + ".json", FileMode.OpenOrCreate | FileMode.Truncate);
            JsonSerializer.Serialize(st, parma);
            st.DisposeAsync();
            return new RunResult(ErrorCode.VisionToolSaveParmaSuccess);
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.VisionToolFailToSaveParma, ex);
        }
        
    }

    public override RunResult Run(HImage image, InParma inParma, OutParma? outParma)
    {
        outParma = null;
        if (image.IsInitialized())
        {
            return new RunResult(ErrorCode.VisionToolImageInvalid);
        }
        else
        {
            for (int i = 0; i < parma.RegionCount; i++)
            {

            }
        }
    }

    private RunResult Action(HImage image, int index)
    {
    }

    private bool ActionForBarCode(HImage image,int index)
    {
        bool flag = this.parma.ActionParmas[index] != null &&
                    this.parma.ActionParmas[index].BarCodeEnable &&
                    this.parma.ActionParmas[index].CheckCodeRegion.IsInitialized() &&
                    image != null &&
                    image.IsInitialized();
        if (flag)
        {
            // 剪切出检测区域
            HOperatorSet.ReduceDomain(image, this.parma.ActionParmas[index].CheckCodeRegion, out HObject imageReduced);
            for (int i = 0; i < 3; i++)
            {
                HOperatorSet.FindBarCode(imageReduced,
                                         out HObject symbolRegions,
                                         this.BarHandles[i],
                                         parma.ActionParmas[i].BarCodeType.GetString(),
                                         out HTuple decodedDataStrings);
                HOperatorSet.AreaCenter(symbolRegions, out HTuple area, out HTuple row, out HTuple col);
            }
        }
        else
        {
            return false;
        }
    }

    private RunResult CodeToolParmaInit()
    {
        bool[] flag = new bool[3];
        Exception? exz = default;

        for (int i = 0; i < 3; i++)
        {
            try
            {
                HOperatorSet.SetBarCodeParam(BarHandles[i], "element_size_min", parma.barCodeParmas[i].BarElementSize);
                HOperatorSet.SetBarCodeParam(BarHandles[i], "meas_thresh", parma.barCodeParmas[i].BarElementSize);
                HOperatorSet.SetBarCodeParam(BarHandles[i], "meas_thresh_abs", parma.barCodeParmas[i].BarElementSize);
                flag[i] = true;
            }
            catch (Exception ex)
            {
                flag[i] = false;
                exz = ex;
            }
        }
        if (flag.Contains<bool>(false))
        {
            Log.GetLogger(LogKeyword.VisionTool).Error(ErrorCode.VisionToolFailToInit.GetString(),
                                                       "条形码扫码参数赋值失败",
                                                       exz);
            return new RunResult(ErrorCode.VisionToolFailToInit, "条形码扫码参数赋值失败", exz);
        }
        return new RunResult(ErrorCode.VisionToolInitSuccess);
    }
}