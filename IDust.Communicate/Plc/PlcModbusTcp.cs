using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Core.IMessage;
using Microsoft.Extensions.Logging;
using IDust.Base;

namespace IDust.Communicate.Plc;

public class PlcModbusTcp : PlcBase
{
    #region member
    private ModbusTcpNet modbusTcpNet;
    private ILogger<PlcModbusTcp> _logger;
    #endregion

    #region ctor
    public PlcModbusTcp(PlcParma parma, ILogger<PlcModbusTcp> logger)
    {
        this.parma = parma;
        modbusTcpNet = new ModbusTcpNet();
        _logger = logger;
    }
    #endregion

    #region public method
    public virtual string GetIpAddress()
    {
        return parma.IpAddress;
    }
    #endregion

    #region private method
    private void setPlcDataFormat()
    {
        modbusTcpNet.DataFormat = parma.DataFormat;
    }

    private RunResult Init()
    {
        string msg = "初始化 Modbus 失败.";
        RunResult r = new RunResult(msg);
        try
        {
            modbusTcpNet?.ConnectClose();
            byte.TryParse(parma.Station.ToString(), out byte station);
            modbusTcpNet = new ModbusTcpNet(parma.IpAddress, parma.Port, station);
            modbusTcpNet.ConnectTimeOut = parma.ConnectTimeOut;
            modbusTcpNet.ReceiveTimeOut = parma.ReceiveTimeOut;
            r.SetSuccess("初始化 Modbus 成功.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, msg);
        }
        return r;
    }
    #endregion

    #region override method
    public override RunResult SetStation(int station)
    {
        RunResult r = new RunResult("设置站号失败.");
        parma.Station = station;
        if (modbusTcpNet != null)
        {
            modbusTcpNet.Station = (byte)parma.Station;
            r.SetSuccess("设置站号成功.");
        }
        return r;
    }

    public override RunResult ConnectServer()
    {
        RunResult init_result = Init();
        if (init_result.isSuccess)
        {
            OperateResult connect_result = modbusTcpNet.ConnectServer();
            setPlcDataFormat();
            modbusTcpNet.IsStringReverse = parma.StrReverse;
            if (connect_result.IsSuccess)
            {
                SetStation(parma.Station);
                this._connectStatus = true;
                init_result.message = "Modbus 连接成功.";
                init_result.errorCode = ErrorCode.OK;
            }
            else
            {
                return RunResult.CreateFailResult(connect_result.ToMessageShowString());
            }
        }
    }
    #endregion
}