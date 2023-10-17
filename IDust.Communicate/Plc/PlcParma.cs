using HslCommunication.Core;
using HslCommunication.Profinet.Delta;
using IDust.Base;
using System.IO.Ports;

namespace IDust.Communicate.Plc;

/// <summary>
/// PLC 的品牌
/// </summary>
public enum PlcBrand
{
    /// <summary>
    /// 台达
    /// </summary>
    Delta,

    /// <summary>
    /// 西门子
    /// </summary>
    Siemens,

    /// <summary>
    /// 无限定品牌，通信协议采用 modbus TCP
    /// </summary>
    ModbusTcp,

    /// <summary>
    /// 无限定品牌，通信协议采用 modbus RTU
    /// </summary>
    ModbusRtu,

    /// <summary>
    /// 无限定品牌，通信协议采用 modbus Ascii
    /// </summary>
    ModbusAscii,

    /// <summary>
    /// 欧米龙
    /// </summary>
    OmronFinsTcp,

    /// <summary>
    /// 无限定品牌，通信协议采用 modbus RTU over TCP
    /// </summary>
    ModbusRtuOverTcp,

    /// <summary>
    /// 三菱 Ascii
    /// </summary>
    MelsecAscii,

    /// <summary>
    /// 三菱 二进制通信
    /// </summary>
    MelsecBinary
}

/// <summary>
/// PLC 的类型
/// </summary>
public enum PlcType
{
    Delta_AH,
    Delta_DVP,
    Delta_RTU,
    Siemens_S200Smart,
    Siemens_S300,
    Siemens_S400,
    Siemens_S1200,
    Siemens_S1500
}


/// <summary>
/// 与 PLC 通信时使用的参数
/// </summary>
public struct PlcParma
{
    /// <summary>
    /// PLC 的品牌
    /// </summary>
    public PlcBrand Brand = PlcBrand.Delta;

    /// <summary>
    /// PLC 的类型
    /// </summary>
    public PlcType plc_type = PlcType.Delta_AH;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName = "PLC 1";
    /// <summary>
    /// 站号
    /// </summary>
    public int Station = 0;
    /// <summary>
    /// 字符串是否反转
    /// </summary>
    public bool StrReverse = true;
    /// <summary>
    /// 数据格式
    /// </summary>
    public DataFormat DataFormat = DataFormat.CDAB;
    /// <summary>
    /// 用户自定义标签序号，用来表示第几个PLC
    /// <para>从0开始</para>
    /// </summary>
    public int UserDefineIndex = 0;
    /// <summary>
    /// 是否自动重连
    /// </summary>
    public bool ReconnectionEnable = true;
    /// <summary>
    /// 重连地址
    /// </summary>
    public string CheckReconnectAddress = "100";
    /// <summary>
    /// 通信所用端口
    /// </summary>
    public int Port = 502;
    /// <summary>
    /// 通信地址
    /// </summary>
    public string IpAddress = "192.169.1.1";
    /// <summary>
    /// 反馈超时时间 ms
    /// </summary>
    public int ReceiveTimeOut = 2000;
    /// <summary>
    /// 连接超时时间 ms
    /// </summary>
    public int ConnectTimeOut = 2000;

    /// <summary>
    /// 串口设置的参数
    /// </summary>
    public SerialPortParma serialPortParma = new SerialPortParma();

    public bool IsShortConnect = false;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PlcParma() { }
}