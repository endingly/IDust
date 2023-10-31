using HalconDotNet;
using IDust.Base;
using IDust.Vision.Tools.CodeScan;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IDust.Vision.Tools.CodeScan;

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
            parma = r;
        }
        else
        {
            parma = new ScanCodeToolParma();
            Log.GetLogger(LogKeyword.VisionTool).Error(ErrorCode.VisionToolFailToInit.GetString());
        }
        if (!Directory.Exists(ToolWorkDir))
        {
            Directory.CreateDirectory(ToolWorkDir);
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
            if (parma.CodeType == CodeType.BarCode)
            {
                for (int i = 0; i < 3; i++)
                    HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out BarHandles[i]);
            }
            else if (parma.CodeType == CodeType.QRCode)
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

    private bool ActionForBarCode(HImage image, int index)
    {
        // 首先，条形码参数不为空，图像不为空且检测区域不为空
        bool flag = parma.ActionParmas[index] != null &&
                    parma.ActionParmas[index].CheckCodeRegion.IsInitialized() &&
                    image != null &&
                    image.IsInitialized();
        if (flag)
        {
            // 初始化结果容器
            List<string> resultStrings = new List<string>();
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<HObject> resultRegions = new List<HObject>();
            HObject ResultImage = new(image);
            // 剪切出检测区域
            HOperatorSet.ReduceDomain(image,
                                      parma.ActionParmas[index].CheckCodeRegion,
                                      out HObject imageReduced);
            for (int i = 0; i < 3; i++)
            {
                // 寻找条形码
                HOperatorSet.FindBarCode(imageReduced,
                                         out HObject symbolRegions,
                                         BarHandles[i],
                                         ((ScanBarCodeParma)parma.ActionParmas[i].ScanCodeParma).BarCodeType.GetString(),
                                         out HTuple decodedDataStrings);
                // 标出条码中心
                HOperatorSet.AreaCenter(symbolRegions, out HTuple area, out HTuple row, out HTuple col);
                // 收集扫码的结果：区域
                for (int j = 0; j < symbolRegions.CountObj(); j++)
                    resultRegions.Add(symbolRegions[j + 1]);
                // 在图像上显示区域
                foreach (var item in resultRegions)
                {
                    HOperatorSet.PaintRegion(item, ResultImage, out HObject ResultImage1, 0, "fill");
                    HOperatorSet.PaintRegion(item, ResultImage, out HObject ResultImage2, 255, "fill");
                    HOperatorSet.PaintRegion(item, ResultImage, out HObject ResultImage3, 0, "fill");
                    HOperatorSet.Compose3(ResultImage1, ResultImage2, ResultImage3,out ResultImage);
                }
                    
                    
                

                // 收集扫码的结果：条码内容\条码中心
                if (decodedDataStrings.Length > 0)
                {
                    resultStrings.AddRange(decodedDataStrings.SArr);
                    x.AddRange(row.DArr);
                    y.AddRange(col.DArr);
                }
                string[] rstring = resultStrings.GroupBy(x => x).Select(x => x.Key).ToArray();


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
        if (flag.Contains(false))
        {
            Log.GetLogger(LogKeyword.VisionTool).Error(ErrorCode.VisionToolFailToInit.GetString(),
                                                       "条形码扫码参数赋值失败",
                                                       exz);
            return new RunResult(ErrorCode.VisionToolFailToInit, "条形码扫码参数赋值失败", exz);
        }
        return new RunResult(ErrorCode.VisionToolInitSuccess);
    }
}