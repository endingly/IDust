using HalconDotNet;
using IDust.Base;

namespace IDust.Vision.Tools.CodeScan;
using ScanResultPackages = List<CodeScanResult>;

/// <summary>
/// 条形码特有参数
/// </summary>
public class BarCodeAction : CodeActionBase
{
    /// <summary>
    /// 条形码参数的 Halcon 句柄
    /// </summary>
    public HBarCode hBarCode;

    /// <summary>
    /// 检测阈值
    /// <para>有关参数的范围详见 <see href="https://www.mvtec.com/doc/halcon/13/en/set_bar_code_param.html">Halcon 文档</see></para>
    /// </summary>
    public double BarMeasThresh
    {
        get
        {
            return this.hBarCode.GetBarCodeParam("meas_thresh");
        }
        set
        {
            if (value <= 0.02 && value >= 0.5)
                this.hBarCode.SetBarCodeParam("meas_thresh", value);
        }
    }
    /// <summary>
    /// 绝对检测阈值
    /// <para>有关参数的范围详见 <see href="https://www.mvtec.com/doc/halcon/13/en/set_bar_code_param.html">Halcon 文档</see></para>
    /// </summary>
    public double BarMeasThreshAbs
    {
        get
        {
            return this.hBarCode.GetBarCodeParam("meas_thresh_abs");
        }
        set
        {
            if (value <= 0.0 && value >= 10.0)
                this.hBarCode.SetBarCodeParam("meas_thresh_abs", value);
        }
    }
    /// <summary>
    /// 检测目标大小的最小值
    /// <para>有关参数的范围详见 <see href="https://www.mvtec.com/doc/halcon/13/en/set_bar_code_param.html">Halcon 文档</see></para>
    /// </summary>
    public double BarElementSize
    {
        get
        {
            return this.hBarCode.GetBarCodeParam("element_size_min");
        }
        set
        {
            if (value <= 1 && value >= 64)
                this.hBarCode.SetBarCodeParam("element_size_min", value);
        }
    }

    /// <summary>
    /// 默认无参构造函数
    /// <para>默认参数为：</para>
    /// <para><c>element_size_min:2.0</c></para>
    /// <para><c>element_size_max:8.0</c></para>
    /// <para><c>meas_thresh:0.05</c></para>
    /// <para><c>meas_thresh_abs:5</c></para>
    /// 具体参见 <see href="https://www.mvtec.com/doc/halcon/13/en/set_bar_code_param.html">Halcon 文档</see>
    /// </summary>
    public BarCodeAction()
    {
        hBarCode = new HBarCode(new HTuple(), new HTuple());
    }

    /// <summary>
    /// 有参构造函数，参数应指定在范围中，否则不生效
    /// <para>有关参数的范围详见 <see href="https://www.mvtec.com/doc/halcon/13/en/set_bar_code_param.html">Halcon 文档</see></para>
    /// </summary>
    /// <param name="element_size_min">元素的最小大小[1~64]</param>
    /// <param name="meas_thresh">边缘幅值[0.02~0.5]</param>
    /// <param name="meas_thresh_abs">相对边缘幅值[0.0~10.0]</param>
    public BarCodeAction(double element_size_min, double meas_thresh, double meas_thresh_abs)
    {
        hBarCode = new HBarCode(new HTuple(), new HTuple());
        if (element_size_min <= 1 && element_size_min >= 64)
            hBarCode.SetBarCodeParam("element_size_min", element_size_min);
        if (meas_thresh <= 0.02 && meas_thresh >= 0.5)
            hBarCode.SetBarCodeParam("meas_thresh", meas_thresh);
        if (meas_thresh_abs <= 0.0 && meas_thresh_abs >= 10.0)
            hBarCode.SetBarCodeParam("meas_thresh_abs", meas_thresh_abs);
    }

    public override string ToString()
    {
        return $"BarMeasThresh:{BarMeasThresh},BarMeasThreshAbs:{BarMeasThreshAbs},BarElementSize:{BarElementSize}";
    }

    /// <summary>
    /// Note: 在一个人为制造的区域中可能有多个条码
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public override RunResult<ScanResultPackages> Run(ref HImage image)
    {
        // 首先图片要规则
        if (image.IsInitialized())
        {
            // 结果图像
            ScanResultPackages results = new ScanResultPackages();
            HObject imageReduced;
            if (CheckCodeRegion == null)
                imageReduced = image;
            else
                HOperatorSet.ReduceDomain(image,
                                          CheckCodeRegion,
                                          out imageReduced);
            // 寻找条形码
            var Rregion = this.hBarCode.FindBarCode(new HImage(imageReduced),
                                                    this.CodeType.GetString(),
                                                    out HTuple decodedDataStrings);
            if (Rregion == null)
            {
                return new RunResult<ScanResultPackages>(ErrorCode.VisionToolFailToRun);
            }
            // 收集扫码的结果，因为可能存在多个区域，所以需要一个循环
            for (int i = 0; i < Rregion.Area.Length; i++)
            {
                var r = new CodeScanResult();
                r.Region = Rregion[i + 1];
                r.Region.AreaCenter(out r.RegionCenter_x, out r.RegionCenter_y);
                if (decodedDataStrings.Length > 0)
                    r.DataString = decodedDataStrings.SArr[i];
                else
                    r.DataString = string.Empty;
                results.Add(r);
            }
            // 后处理
            AfterRun(results, ref image);
            return new RunResult<ScanResultPackages>(ErrorCode.VisionToolRunSuccess, results);
        }
        else
        {
            return new RunResult<ScanResultPackages>(ErrorCode.VisionToolFailToRun);
        }
    }
}
