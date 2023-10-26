using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Calibrating;
/// <summary>
/// 标定所使用的数据参数，可序列化至文件
/// </summary>
[Serializable]
public struct CalibrateDataParma
{
    public Deviation RobotDeviation;

    public Point[] RobotPoints;

    public Point[] ImagePoints;

    
}

