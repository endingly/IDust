using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Core.IMessage;
using IDust.Base;
using System.Text;

namespace IDust.Communicate.Plc;
#pragma warning disable CS8500  // 确认以下操作皆为 struct 对象，故而此警告可忽略

public class PlcModbusTcp : PlcBase
{
    #region member
    private ModbusTcpNet modbusTcpNet;
    #endregion

    #region ctor
    public PlcModbusTcp(in PlcParma parma)
    {
        this.parma = parma;
        modbusTcpNet = new ModbusTcpNet();
        timer.Elapsed += reConnect;
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
        modbusTcpNet.DataFormat = (DataFormat)parma.DataFormat;
    }

    private RunResult Init()
    {
        RunResult r = new RunResult();
        try
        {
            modbusTcpNet?.ConnectClose();
            byte.TryParse(parma.Station.ToString(), out byte station);
            modbusTcpNet = new ModbusTcpNet(parma.IpAddress, parma.Port, station)
            {
                ConnectTimeOut = parma.ConnectTimeOut,
                ReceiveTimeOut = parma.ReceiveTimeOut
            };
            r.Reset(ErrorCode.PlcInitSuccess);
        }
        catch (Exception ex)
        {
            modbusTcpNet.LogNet.WriteException(LogKeyWord.PLC.GetString(),
                                               ErrorCode.PlcFailToInit.GetString(),
                                               ex);
        }
        return r;
    }

    /// <summary>
    /// 重新连接
    /// </summary>
    /// <returns></returns>
    protected override void reConnect(object sender, System.Timers.ElapsedEventArgs e)
    {
        modbusTcpNet.IpAddress = parma.IpAddress;
        modbusTcpNet.Port = parma.Port;
        var r = SetStation(parma.Station);
        if (!r)
        {
            return;
        }
        modbusTcpNet.ConnectTimeOut = parma.ConnectTimeOut;
        modbusTcpNet.IsStringReverse = parma.StrReverse;
        var or = modbusTcpNet.ConnectServer();
        if (or.IsSuccess)
        {
            this.Status = true;
            modbusTcpNet.LogNet.WriteInfo(ErrorCode.PlcReconnectSuccess.GetString());
        }
        else
        {
            this.Status = false;
            modbusTcpNet.LogNet.WriteError(ErrorCode.PlcFailToReconnect.GetString());
        }
    }
    #endregion

    #region override method
    public override bool SetStation(int station)
    {
        if (modbusTcpNet != null)
        {
            modbusTcpNet.Station = (byte)parma.Station;
            return true;
        }
        return false;
    }

    public override RunResult ConnectServer()
    {
        RunResult init_result = Init();
        if (init_result.isSuccess)
        {
            OperateResult connect_result = modbusTcpNet.ConnectServer();
            modbusTcpNet.DataFormat = parma.DataFormat;
            modbusTcpNet.IsStringReverse = parma.StrReverse;
            if (connect_result.IsSuccess)
            {
                SetStation(parma.Station);
                this.Status = true;
                init_result.Reset(ErrorCode.PlcConnected);
            }
            else
            {
                this.Status = false;
                init_result.Reset(ErrorCode.PlcNotConnect);
            }
            return init_result;
        }
        return init_result;
    }

    public override RunResult ReadValue(string address, int length, out string value)
    {
        value = string.Empty;
        OperateResult<string> operate_result = modbusTcpNet.ReadString(address, (ushort)length);
        if (operate_result.IsSuccess)
        {
            value = operate_result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        return new RunResult(ErrorCode.PlcFailToRead);
    }

    public override RunResult ReadValue<T>(string address, int length, out T[] value)
    {
        value = [];
        unsafe
        {
            var vsize = sizeof(T);
            OperateResult<byte[]> operate_result = modbusTcpNet.Read(address, (ushort)(length * vsize));
            if (operate_result.IsSuccess)
            {
                value = new T[length];
                fixed (byte* p = operate_result.Content)
                {
                    for (int i = 0; i < length; i++)
                    {
                        value[i] = *(T*)(p + i * vsize);
                    }
                }
                return new RunResult(ErrorCode.PlcReadSuccess);
            }
        }
        return new RunResult(ErrorCode.PlcFailToRead);
    }

    public override RunResult ReadValue<T>(string address, out T value)
    {
        value = default;
        unsafe
        {
            var vsize = sizeof(T);
            OperateResult<byte[]> operate_result = modbusTcpNet.Read(address, (ushort)vsize);
            if (operate_result.IsSuccess)
            {
                fixed (byte* p = operate_result.Content)
                {
                    value = *(T*)p;
                }
                return new RunResult(ErrorCode.PlcReadSuccess);
            }
        }
        return new RunResult(ErrorCode.PlcFailToRead);
    }

    public override RunResult ReadUnicodeString(string address, int length, out string value)
    {
        value = string.Empty;
        OperateResult<byte[]> operate_result = modbusTcpNet.Read(address, (ushort)length);
        if (operate_result.IsSuccess)
        {
            value = modbusTcpNet.ByteTransform.TransString(operate_result.Content, Encoding.Unicode);
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        return new RunResult(ErrorCode.PlcFailToRead);
    }

    public override RunResult WriteValue<T>(string address, T value)
    {
        unsafe
        {
            var vsize = sizeof(T);
            byte[] buffer = new byte[vsize];
            fixed (byte* p = buffer)
            {
                *(T*)p = value;
            }
            OperateResult operate_result = modbusTcpNet.Write(address, buffer);
            if (operate_result.IsSuccess)
            {
                return new RunResult(ErrorCode.PlcWriteSuccess);
            }
        }
        return new RunResult(ErrorCode.PlcFailToWrite);
    }

    public override RunResult WriteValue<T>(string address, int length, T[] value)
    {
        unsafe
        {
            var vsize = sizeof(T);
            byte[] buffer = new byte[length * vsize];
            fixed (byte* p = buffer)
            {
                for (int i = 0; i < length; i++)
                {
                    *(T*)(p + i * vsize) = value[i];
                }
            }
            OperateResult operate_result = modbusTcpNet.Write(address, buffer);
            if (operate_result.IsSuccess)
            {
                return new RunResult(ErrorCode.PlcWriteSuccess);
            }
        }
        return new RunResult(ErrorCode.PlcFailToWrite);
    }

    public override RunResult WriteValue(string address, int length, string value)
    {
        OperateResult operate_result = modbusTcpNet.Write(address, value);
        if (operate_result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        return new RunResult(ErrorCode.PlcFailToWrite);
    }

    public override RunResult WriteUnicodeString(string address, int length, string value)
    {
        OperateResult operate_result = modbusTcpNet.Write(address, value, Encoding.Unicode);
        if (operate_result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        return new RunResult(ErrorCode.PlcFailToWrite);
    }

    public override RunResult ClearValue(string address, int length)
    {
        byte[] buffer = new byte[length];
        Array.Fill(buffer, byte.MinValue);
        var r = WriteValue(address, length, buffer);
        if (r.isSuccess)
        {
            r.Reset(ErrorCode.PlcClearDataSuccess);
            return r;
        }
        else
        {
            r.Reset(ErrorCode.PlcFailToClearData);
            return r;
        }
    }

    public override RunResult ConnectClose()
    {
        var r = modbusTcpNet?.ConnectClose();
        if (r != null && r.IsSuccess)
        {
            Status = false;     // 触发事件
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect);
        }
    }
    #endregion
}