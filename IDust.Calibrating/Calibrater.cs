using System.Net.Mime;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Calibrating;

/// <summary>
/// 用于标定的计算算子合集
/// </summary>
public class Calibrater
{
    #region member
    /// <summary>
    /// 变换用的仿射矩阵
    /// </summary>
    public HTuple? homMat2D;

    /// <summary>
    /// 待处理的图像宽度
    /// </summary>
    private int image_width;

    /// <summary>
    /// 待处理的图像高度
    /// </summary>
    private int image_height;

    /// <summary>
    /// 此标定所使用的相机标定类型
    /// </summary>
    private CalibrateApplyType Ctype;

    /// <summary>
    /// 模板所使用的仿射矩阵存储路径
    /// </summary>
    private readonly string homMat_storePath;

    /// <summary>
    /// 现在传入数据所计算出来的仿射矩阵存储路径
    /// </summary>
    private readonly string homMatNew_storePath;

    /// <summary>
    /// 其他标定数据存储路径
    /// </summary>
    private readonly string calibraterData_storePath;
    #endregion

    #region constructor
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="calibrate_name"></param>
    /// <param name="type"></param>
    /// <param name="image_width"></param>
    /// <param name="image_height"></param>
    public Calibrater(string calibrate_name, CalibrateApplyType type, int image_width = 3072, int image_height = 2048)
    {
        string s = Environment.CurrentDirectory;
        homMat_storePath = Path.Combine(s, calibrate_name, "CalHomMat.tup");
        homMatNew_storePath = Path.Combine(s, calibrate_name, "CalHomMatNew.tup");
        calibraterData_storePath = Path.Combine(s, calibrate_name, "CalData.json");
        Ctype = type;
    }
    #endregion

    #region static function
    /// <summary>
    /// 从两条线以及一个已知点中计算变换后的点位置以及角度
    /// </summary>
    /// <param name="line_one">第一条线</param>
    /// <param name="line_two">第二条线</param>
    /// <param name="point">需要变换的点</param>
    public static Tuple<double, double, double> GetShapesAngleFrom_llp(in Line line_one, in Line line_two, in Point point)
    {
        // 前两者是函数的输入，要求必须至少有两个点
        HTuple px = new HTuple();
        HTuple py = new HTuple();
        HTuple qx = new HTuple();
        HTuple qy = new HTuple();

        px.Append(line_one.start.x);
        px.Append(line_one.end.x);
        py.Append(line_one.start.y);
        py.Append(line_one.end.y);

        qx.Append(line_two.start.x);
        qx.Append(line_two.end.x);
        qy.Append(line_two.start.y);
        qy.Append(line_two.end.y);

        // 根据变换前的线条以及变换后后的线条计算仿射矩阵
        HOperatorSet.VectorToRigid(px, py, qx, qy, out HTuple homMat2D);
        // 得到放射矩阵之后，计算变换之后的中心
        HOperatorSet.AffineTransPoint2d(homMat2D, point.x, point.y, out HTuple rp_x, out HTuple rp_y);
        // 计算变换的角度
        double angle = GetAngleFromTwoLines(in line_one, in line_two);
        Tuple<double, double, double> tuple = new(rp_x, rp_y, angle);

        return tuple;
    }

    /// <summary>
    /// 从两条线中获取夹角的角度
    /// </summary>
    /// <param name="line_one"></param>
    /// <param name="line_two"></param>
    /// <returns></returns>
    public static double GetAngleFromTwoLines(in Line line_one, in Line line_two)
    {
        HOperatorSet.AngleLl(line_one.start.x, line_one.start.y, line_one.end.x, line_one.end.y,
                             line_two.start.x, line_two.start.y, line_two.end.x, line_two.end.y,
                             out HTuple angle);
        return angle;
    }
    #endregion

    #region property
    /// <summary>
    /// 获取或设置标定所使用标定类型
    /// </summary>
    public CalibrateApplyType CalibrateApplyType
    {
        get => Ctype;
        set => Ctype = value;
    }
    #endregion
}

