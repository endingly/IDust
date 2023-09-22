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
    /// <summary>
    /// 从两条线以及一个已知点中计算变换后的点位置以及角度
    /// </summary>
    /// <param name="line_one">第一条线</param>
    /// <param name="line_two">第二条线</param>
    /// <param name="point">需要变换的点</param>
    public static void GetShapesAngleFrom_llp(in Line line_one, in Line line_two, in Point point)
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
}

