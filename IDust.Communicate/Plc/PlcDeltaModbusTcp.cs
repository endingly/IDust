using HslCommunication.Profinet.Delta;
using IDust.Base;
using System.Runtime.InteropServices;
using System.Timers;

namespace IDust.Communicate.Plc;

public partial class PlcDeltaModbusTcp : PlcModbusTcp
{
    private string _deltaPlcType;

    public string DeltaPlcType
    {
        get { return _deltaPlcType; }
        private set
        {
            if (value != "AH" && value != "DVP" && value != "RTU")
            {
                throw new ArgumentException("台达 PLC 类型错误");
            }
            else
            {
                _deltaPlcType = value;
            }
        }
    }

    public PlcDeltaModbusTcp(in PlcParma parma, string delta_type) : base(parma)
    {
        this.DeltaPlcType = delta_type;
    }

    public PlcDeltaModbusTcp(in PlcParma parma) : base(parma)
    {
        this.DeltaPlcType = "AH";
    }

    [LibraryImport("DMT.dll", StringMarshalling = StringMarshalling.Utf8)]
    private static partial int DevToAddrW(string delta_plc_type, string device_adress, int flag = 1);

    /// <summary>
    /// 获取设备地址
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public string GetDevToAddress(string device) => DevToAddrW(this.DeltaPlcType, device).ToString();
}

