using HslCommunication;
using HslCommunication.ModBus;
using IDust.Base;
using IDust.Communicate.Plc;
using System.Timers;

namespace IDust.Communicate;

public class PlcModbusAscii : PlcBase
{
    #region Members
    private ModbusAscii client;
    #endregion

    #region ctor
#pragma warning disable CS8618,CS8602
    public PlcModbusAscii(in PlcParma parma)
    {
        this.parma = parma;
        var r = Init();
        if (r.isSuccess)
        {
            client.DataFormat = parma.DataFormat;
        }
    }
#pragma warning restore CS8618
    #endregion

    #region private methods
    private RunResult Init()
    {
        RunResult r = new RunResult();
        try
        {
            byte.TryParse(parma.Station.ToString(), out byte station);
            if (client != null)
            {
                client.Close();
            }
            else
            {
                client = new ModbusAscii(station);
            }
            client.ReceiveTimeout = parma.ReceiveTimeOut;
            client.SerialPortInni(sp =>
            {
                sp.PortName = parma.serialPortParma.PortName;
                sp.BaudRate = parma.serialPortParma.BaudRate;
                sp.DataBits = parma.serialPortParma.DataBits;
                sp.StopBits = parma.serialPortParma.StopBits;
                sp.Parity = parma.serialPortParma.Parity;
            });
            r.Reset(ErrorCode.PlcInitSuccess);
        }
        catch (Exception ex)
        {
            client.LogNet.WriteException(LogKeyWord.PLC.GetString(),
                                         ErrorCode.PlcFailToInit.GetString(),
                                         ex);
            r.Reset(ErrorCode.PlcFailToInit);
        }
        return r;
    }
    #endregion

    #region override methods
    public override bool SetStation(int station)
    {
        parma.Station = station;
        if (client != null)
        {
            client.Station = (byte)station;
            return true;
        }
        return false;
    }

    public override RunResult ConnectServer()
    {
        var r = Init();
        if (r.isSuccess && !parma.IsShortConnect)
        {
            try
            {
                client.Open();
                Status = client.IsOpen();
                if (Status)
                {
                    SetStation(parma.Station);
                    client.LogNet.WriteInfo(ErrorCode.PlcConnected.GetString());
                    return new RunResult(ErrorCode.PlcConnected);
                }
                else
                {
                    client.LogNet.WriteError(ErrorCode.PlcNotConnect.GetString());
                    return new RunResult(ErrorCode.PlcNotConnect);
                }
            }
            catch (Exception ex)
            {
                return new RunResult(ErrorCode.PlcNotConnect, ex);
            }
        }
        return r;
    }

    protected override void reConnect(object sender, ElapsedEventArgs e)
    {
        var r = Init();
        if (r.isSuccess)
        {
            return;
        }
        var or = client.Open();
        if (or.IsSuccess)
        {
            this.Status = true;
            client.LogNet.WriteInfo(ErrorCode.PlcReconnectSuccess.GetString());
        }
        else
        {
            this.Status = false;
            client.LogNet.WriteError(ErrorCode.PlcFailToReconnect.GetString());
        }
    }

    public override RunResult ConnectClose()
    {
        try
        {
            client.Close();
            Status = client.IsOpen();
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect, ex);
        }
    }

    public override RunResult ReadValue(string address, int length, out string value)
    {
        var r = client.ReadString(address, (ushort)length);
        if (r.IsSuccess)
        {
            value = r.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = string.Empty;
            client.LogNet.WriteError(IDust.Base.LogKeyWord.PLC.GetString(),
                                     ErrorCode.PlcFailToRead.GetString() + "->" + r.ToMessageShowString());
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }

    public override RunResult ReadValue<T>(string address, out T value)
    {
        value = default;
        unsafe
        {
            var vsize = sizeof(T);
            OperateResult<byte[]> operate_result = client.Read(address, (ushort)vsize);
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

    public override RunResult ReadValue<T>(string address, int length, out T[] value)
    {
        value = [];
        unsafe
        {
            var vsize = sizeof(T);
            OperateResult<byte[]> operate_result = client.Read(address, (ushort)(length * vsize));
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
            OperateResult operate_result = client.Write(address, buffer);
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
            OperateResult operate_result = client.Write(address, buffer);
            if (operate_result.IsSuccess)
            {
                return new RunResult(ErrorCode.PlcWriteSuccess);
            }
        }
        return new RunResult(ErrorCode.PlcFailToWrite);
    }

    public override RunResult WriteValue(string address, int length, string value)
    {
        OperateResult operate_result = client.Write(address, value);
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
    #endregion
}
